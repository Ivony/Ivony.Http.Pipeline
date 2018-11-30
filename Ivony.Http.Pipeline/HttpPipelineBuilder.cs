using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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

        ApplyResponse( context, response );
      };

    }

    protected virtual void ApplyResponse( HttpContext context, HttpResponseMessage response )
    {
      throw new NotImplementedException();
    }

    protected virtual HttpRequestMessage CreateRequest( HttpContext context )
    {
      var request = new HttpRequestMessage( new HttpMethod( context.Request.Method ), CreateUri( context.Request ) );
      foreach ( var item in context.Request.Headers )
        request.Headers.Add( item.Key, item.Value.AsEnumerable() );

      request.Content = new StreamContent( context.Request.Body );
      request.Properties[HttpContextAccessKey] = context;

      return request;


    }

    protected virtual Uri CreateUri( HttpRequest request )
    {
      throw new NotImplementedException();
    }
  }
}