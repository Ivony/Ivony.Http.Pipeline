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
  /// 辅助在 ASP.NET Core 上创建 HTTP 管线
  /// </summary>
  public class HttpPipelineAspNetCoreProvider : IHttpPipeline
  {
    internal static readonly string HttpContextAccessKey = "__HttpContext";


    private HttpPipelineHandler _pipeline;


    /// <summary>
    /// 链接下游管线
    /// </summary>
    /// <param name="pipeline">下游管线</param>
    /// <returns>返回下游管线</returns>
    public HttpPipelineHandler Pipe( HttpPipelineHandler pipeline )
    {
      return _pipeline = pipeline;
    }



    /// <summary>
    /// 创建 ASP.NET Core 中间件
    /// </summary>
    /// <returns>ASP.NET Core 中间件</returns>
    public Func<RequestDelegate, RequestDelegate> BuildMiddleware()
    {

      if ( _pipeline == null )
        throw new InvalidOperationException();


      return continuation => async context =>
      {

        var request = CreateRequest( context );

        var response = await _pipeline( request );

        await ApplyResponse( context, response );
      };

    }

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


    protected virtual HttpRequestMessage CreateRequest( HttpContext context )
    {
      var request = new HttpRequestMessage();
      request.Properties[HttpContextAccessKey] = context;

      return request;
    }

  }
}