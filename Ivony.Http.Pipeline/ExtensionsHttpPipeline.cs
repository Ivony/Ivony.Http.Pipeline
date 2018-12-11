using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ivony.Http.Pipeline.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// 提供 HTTP 请求管线的帮助方法
  /// </summary>
  public static class ExtensionsHttpPipeline
  {


    /// <summary>
    /// 让管线分发器处理指定的请求
    /// </summary>
    /// <param name="distributer">管线分发器</param>
    /// <param name="request">要处理的请求</param>
    /// <returns>处理结果</returns>
    public static Task<HttpResponseMessage> Handle( this IHttpPipelineDistributer distributer, HttpRequestMessage request )
    {
      return distributer.Distribute( request )( request );
    }


    /// <summary>
    /// 将管线分发器转换为处理器
    /// </summary>
    /// <param name="distributer">管线分发器</param>
    /// <returns>管线处理器</returns>
    public static HttpPipelineHandler AsHandler( this IHttpPipelineDistributer distributer )
    {
      return request => distributer.Handle( request );
    }



    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="configure">处理管线构建程序</param>
    public static void UsePipeline( this IApplicationBuilder application, Func<IHttpPipeline, HttpPipelineHandler> configure )
    {
      var pipeline = configure( Ivony.Http.Pipeline.HttpPipeline.Blank );
      application.UsePipeline( pipeline );
    }



    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="configure">处理管线构建程序</param>
    public static void UsePipeline( this IApplicationBuilder application, Func<IHttpPipeline, IHttpPipeline> configure )
    {
      var pipeline = configure( Ivony.Http.Pipeline.HttpPipeline.Blank );
      application.UsePipeline( pipeline.Emit( application.ApplicationServices.GetService<IHttpPipelineEmitter>() ) );
    }


    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="pipeline">HTTP 请求处理管线</param>
    public static void UsePipeline( this IApplicationBuilder application, HttpPipelineHandler pipeline )
    {
      var service = application.ApplicationServices.GetService<IHttpPipelineAspNetCoreCombinator>();
      application.Use( service.CreateMiddleware( pipeline ) );
    }


    /// <summary>
    /// get the HttpContext object
    /// </summary>
    /// <param name="request">request message</param>
    /// <returns>HttpContext object</returns>
    public static HttpContext GetHttpContext( this HttpRequestMessage request )
    {
      if ( request.Properties.TryGetValue( HttpPipelineAspNetCoreCombinator.HttpContextAccessKey, out var value ) )
        return (HttpContext) value;

      else
        return null;
    }




  }
}
