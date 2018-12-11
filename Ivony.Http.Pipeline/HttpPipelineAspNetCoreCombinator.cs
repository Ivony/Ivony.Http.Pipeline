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


  public interface IHttpPipelineAspNetCoreCombinator
  {
    Func<RequestDelegate, RequestDelegate> CreateMiddleware( HttpPipelineHandler pipeline );
  }

  /// <summary>
  /// 辅助在 ASP.NET Core 上创建 HTTP 管线
  /// </summary>
  public class HttpPipelineAspNetCoreCombinator : IHttpPipelineAspNetCoreCombinator
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
      var request = new HttpRequestMessage();
      request.Properties[HttpContextAccessKey] = context;

      return Task.FromResult( request );
    }

    public Func<RequestDelegate, RequestDelegate> CreateMiddleware( HttpPipelineHandler pipeline )
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