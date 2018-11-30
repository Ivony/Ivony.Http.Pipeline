using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Ivony.Http.Pipeline
{
  public class HttpPipelineBuilder : IHttpPipelineMiddleware
  {



    internal static readonly string HttpContextAccessKey = "__HttpContext";


    private IHttpPipeline _pipeline;

    public IHttpPipeline Pipe( IHttpPipeline pipeline )
    {
      return _pipeline = pipeline;
    }



    public Func<RequestDelegate, RequestDelegate> Build()
    {

      if ( _pipeline == null )
        throw new InvalidOperationException();


      return continuation => async context =>
      {

        var request = CreateRequest( context );

        var response = await _pipeline.ProcessRequest( request );

        await ApplyResponse( context, response );
      };

    }

    protected virtual async Task ApplyResponse( HttpContext context, HttpResponseMessage response )
    {

      context.Response.StatusCode = (int) response.StatusCode;
      context.Response.Headers.Clear();


      foreach ( var item in response.Headers )
      {
        if ( item.Key == "Connection" )
          continue;

        if ( item.Key == "Transfer-Encoding" )
          continue;

        context.Response.Headers.Add( item.Key, new StringValues( item.Value.ToArray() ) );
      }

      foreach ( var item in response.Content.Headers )
        context.Response.Headers.Add( item.Key, new StringValues( item.Value.ToArray() ) );

      await response.Content.CopyToAsync( context.Response.Body );
    }

    protected virtual HttpRequestMessage CreateRequest( HttpContext context )
    {
      var request = new HttpRequestMessage( new HttpMethod( context.Request.Method ), CreateUri( context.Request ) );
      foreach ( var item in context.Request.Headers )
        request.Headers.Add( item.Key, item.Value.AsEnumerable() );

      /*    
      var stream = new MemoryStream();
      context.Request.Body.CopyTo( stream );
      stream.Seek( 0, SeekOrigin.Begin );

      request.Content = new StreamContent( stream );
      */
      request.Properties[HttpContextAccessKey] = context;

      return request;
    }

    protected virtual Uri CreateUri( HttpRequest request )
    {
      return new UriBuilder
      {
        Scheme = request.Scheme,
        Host = request.Host.Host,
        Port = request.Host.Port.Value,
        Path = request.Path,
        Query = request.QueryString.Value,
      }.Uri;
    }
  }
}