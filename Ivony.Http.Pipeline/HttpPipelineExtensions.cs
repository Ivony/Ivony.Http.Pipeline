using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// 提供 HTTP 请求管线的帮助方法
  /// </summary>
  public static class HttpPipelineExtensions
  {




    /// <summary>
    /// 接入一个管线中间件
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <param name="nextMiddleware">要接入的中间件</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline Pipe( this IHttpPipeline middleware, IHttpPipeline nextMiddleware )
    {
      return new HttpPipelineMiddlewareLink( middleware, nextMiddleware );
    }


    /// <summary>
    /// 接入一个管线中间件
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <param name="nextMiddleware">要接入的中间件</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline Pipe( this IHttpPipeline middleware, Func<HttpPipelineHandler, HttpPipelineHandler> next )
    {
      return new HttpPipelineMiddlewareLink( middleware, new Middleware( next ) );
    }


    private class Middleware : IHttpPipeline
    {
      private readonly Func<HttpPipelineHandler, HttpPipelineHandler> _middleware;

      public Middleware( Func<HttpPipelineHandler, HttpPipelineHandler> middleware )
      {
        _middleware = middleware;
      }

      public HttpPipelineHandler Pipe( HttpPipelineHandler handler )
      {
        return _middleware( handler );
      }
    }


    /// <summary>
    /// 使用负载均衡器
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <param name="pipelines">下游管线列表</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline UseLoadBalancer( this IHttpPipeline middleware, params IHttpPipeline[] pipelines )
    {
      return middleware.Pipe( new HttpPipelineLoadBalancer( pipelines ) );
    }


    /// <summary>
    /// 使用代理转发
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline UseForwardedProxy( this IHttpPipeline middleware )
    {
      return middleware.Pipe( new HttpPipelineForwardedProxy() );
    }


    public static HttpContext GetHttpContext( this HttpRequestMessage request )
    {
      if ( request.Properties.TryGetValue( HttpPipelineBuilder.HttpContextAccessKey, out var value ) )
        return (HttpContext) value;

      else
        return null;
    }










    /// <summary>
    /// 将管线接入发出终结点，创建完整的处理管线
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <returns>完整的处理管线</returns>
    public static HttpPipelineHandler Emit( this IHttpPipeline middleware )
    {
      return middleware.Pipe( new HttpPipelineEmitter() );
    }



    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="configure">处理管线构建程序</param>
    public static void UsePipeline( this IApplicationBuilder application, Action<HttpPipelineBuilder> configure )
    {
      var builder = new HttpPipelineBuilder();
      configure( builder );
      application.Use( builder.Build() );
    }


    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="pipeline">HTTP 请求处理管线</param>
    public static void UsePipeline( this IApplicationBuilder application, IHttpPipeline pipeline )
    {
      var builder = new HttpPipelineBuilder();
      builder.Pipe( pipeline );
      application.Use( builder.Build() );
    }

  }
}
