using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// contract proxy headers build mode
  /// </summary>
  public enum ForwardProxyMode
  {
    /// <summary>just forward the request, not as a proxy</summary>
    None,

    /// <summary>build legacy proxy headers, like X-Forwarded-For, X-Forwarded-Host and X-Forwarded-Proto</summary>
    Legacy,

    /// <summary>build proxy headers using RFC7329</summary>
    RFC7239
  }



  /// <summary>
  /// contract transmit headers behavior
  /// </summary>
  public enum TransmitHeaderBehavior
  {
    /// <summary>dose not transmit any header.</summary>
    Nothing,

    /// <summary>transmit all headers.</summary>
    All
  }


  /// <summary>
  /// 辅助在 ASP.NET Core 上创建 HTTP 管线
  /// </summary>
  public class AspNetCoreCombinator : IHttpPipelineAccessPoint<Func<RequestDelegate, RequestDelegate>>
  {
    internal static readonly string HttpContextAccessKey = "__HttpContext";


    protected virtual async Task ApplyResponse( HttpContext context, HttpResponseMessage response )
    {

      context.Response.StatusCode = (int) response.StatusCode;
      context.Response.Headers.Clear();

      foreach ( var item in response.Headers )
        context.Response.Headers.Add( item.Key, new StringValues( item.Value.ToArray() ) );

      foreach ( var item in response.Content.Headers )
        context.Response.Headers.Add( item.Key, new StringValues( item.Value.ToArray() ) );

      await response.Content.CopyToAsync( context.Response.Body );
    }


    protected virtual Task<HttpRequestMessage> CreateRequest( HttpContext context )
    {
      var request = CreateRequestCore( context );

      request.Method = new HttpMethod( context.Request.Method );
      request.RequestUri = CreateUri( context.Request );


      foreach ( var item in context.Request.Headers )
        request.Headers.Add( item.Key, item.Value.AsEnumerable() );

      return Task.FromResult( request );
    }


    protected virtual Uri CreateUri( HttpRequest request )
    {
      var builder = new UriBuilder();
      builder.Scheme = request.Scheme;
      if ( request.Host.HasValue )
      {
        builder.Host = request.Host.Host;
        if ( request.Host.Port.HasValue )
          builder.Port = request.Host.Port.Value;
      }

      builder.Path = request.Path;
      builder.Query = request.QueryString.Value;

      return builder.Uri;
    }


    private static HttpRequestMessage CreateRequestCore( HttpContext context )
    {
      var request = new HttpRequestMessage();
      request.Properties[HttpContextAccessKey] = context;
      return request;
    }

    Func<RequestDelegate, RequestDelegate> IHttpPipelineAccessPoint<Func<RequestDelegate, RequestDelegate>>.Combine( HttpPipelineHandler pipeline )
    {
      if ( pipeline == null )
        throw new ArgumentNullException( nameof( pipeline ) );


      return continuation => async context =>
      {
        var request = await CreateRequest( context );

        var response = await pipeline( request );

        await ApplyResponse( context, response );
      };
    }
  }
}