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
    /// 接入一个管线
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="nextPipeline">要接入的下游管线</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline Pipe( this IHttpPipeline pipeline, IHttpPipeline nextPipeline )
    {
      return new HttpPipelineMiddlewareLink( pipeline, nextPipeline );
    }


    /// <summary>
    /// 接入一个管线
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="nextMiddleware">要接入的下游管线</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline Pipe( this IHttpPipeline pipeline, Func<HttpPipelineHandler, HttpPipelineHandler> nextPipeline )
    {
      return new HttpPipelineMiddlewareLink( pipeline, new HttpPipeline( nextPipeline ) );
    }


    private class HttpPipeline : IHttpPipeline
    {
      private readonly Func<HttpPipelineHandler, HttpPipelineHandler> _pipeline;

      public HttpPipeline( Func<HttpPipelineHandler, HttpPipelineHandler> pipeline )
      {
        _pipeline = pipeline;
      }

      public HttpPipelineHandler Pipe( HttpPipelineHandler handler )
      {
        return _pipeline( handler );
      }
    }


    /// <summary>
    /// 使用负载均衡器
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="pipelines">下游管线列表</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline UseLoadBalancer( this IHttpPipeline pipeline, params IHttpPipeline[] pipelines )
    {
      return pipeline.Pipe( new HttpPipelineLoadBalancer( pipelines ) );
    }

    /// <summary>
    /// 分发管线
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="dispatcher">分发器</param>
    /// <returns>请求处理管线</returns>
    public static HttpPipelineHandler Dispatch( this IHttpPipeline pipeline, IHttpPipelineDispatcher dispatcher )
    {
      return pipeline.Pipe( request => dispatcher.Dispatch( request )( request ) );
    }

    /// <summary>
    /// 分发管线
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <param name="pipelines">下游管线列表</param>
    /// <returns>请求处理管线</returns>
    public static HttpPipelineHandler Dispatch( this IHttpPipeline pipeline, params HttpPipelineHandler[] pipelines )
    {
      return pipeline.Dispatch( new HttpPipelineBalanceDispatcher( pipelines ) );
    }



    /// <summary>
    /// 让管线分发器处理指定的请求
    /// </summary>
    /// <param name="dispatcher">管线分发器</param>
    /// <param name="request">要处理的请求</param>
    /// <returns>处理结果</returns>
    public static Task<HttpResponseMessage> Handle( this IHttpPipelineDispatcher dispatcher, HttpRequestMessage request )
    {
      return dispatcher.Dispatch( request )( request );
    }


    /// <summary>
    /// 将管线分发器转换为处理器
    /// </summary>
    /// <param name="dispatcher">管线分发器</param>
    /// <returns>管线处理器</returns>
    public static HttpPipelineHandler AsHandler( this IHttpPipelineDispatcher dispatcher )
    {
      return request => dispatcher.Handle( request );
    }



    public static IHttpPipeline UseLogger( this IHttpPipeline pipeline )
    {
      throw new NotImplementedException();
    }


    /// <summary>
    /// 使用代理转发
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline Forward( this IHttpPipeline pipeline )
    {
      return pipeline.Pipe( new HttpPipelineForward() );
    }


    /// <summary>
    /// get the HttpContext object
    /// </summary>
    /// <param name="request">request message</param>
    /// <returns>HttpContext object</returns>
    public static HttpContext GetHttpContext( this HttpRequestMessage request )
    {
      if ( request.Properties.TryGetValue( HttpPipelineAspNetCoreProvider.HttpContextAccessKey, out var value ) )
        return (HttpContext) value;

      else
        return null;
    }





    /// <summary>
    /// 将管道接入请求发出器，创建完整的处理管线
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <returns>完整的处理管线</returns>
    public static HttpPipelineHandler Emit( this IHttpPipeline pipeline )
    {
      return pipeline.Pipe( new HttpPipelineEmitter() );
    }



    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="configure">处理管线构建程序</param>
    public static void UsePipeline( this IApplicationBuilder application, Action<HttpPipelineAspNetCoreProvider> configure )
    {
      var builder = new HttpPipelineAspNetCoreProvider();
      configure( builder );
      application.Use( builder.BuildMiddleware() );
    }


    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="pipeline">HTTP 请求处理管线</param>
    public static void UsePipeline( this IApplicationBuilder application, IHttpPipeline pipeline )
    {
      var builder = new HttpPipelineAspNetCoreProvider();
      builder.Pipe( pipeline );
      application.Use( builder.BuildMiddleware() );
    }

  }
}
