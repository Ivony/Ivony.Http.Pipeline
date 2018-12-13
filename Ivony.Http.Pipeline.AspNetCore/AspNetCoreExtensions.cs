using System;

using Ivony.Http.Pipeline;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Hosting
{
  public static class AspNetCoreExtensions
  {


    /// <summary>
    /// use HTTP pipeline
    /// </summary>
    /// <param name="builder">asp.net core application builder</param>
    /// <returns>HTTP pipeline</returns>
    public static IHttpPipeline UsePipeline( this IApplicationBuilder builder )
    {
      return UsePipeline( builder, builder.ApplicationServices.GetRequiredService<IHttpPipelineAccessPoint<RequestDelegate>>() );
    }



    /// <summary>
    /// use HTTP pipeline
    /// </summary>
    /// <param name="builder">asp.net core application builder</param>
    /// <param name="accessPoint">HTTP pipeline asp.net core access point</param>
    /// <returns>HTTP pipeline</returns>
    public static IHttpPipeline UsePipeline( this IApplicationBuilder builder, IHttpPipelineAccessPoint<RequestDelegate> accessPoint )
    {
      return accessPoint.AsPipeline( application =>
      {
        var logger = builder.ApplicationServices.GetService<ILogger<AspNetCoreCombinator>>();
        if ( logger != null )
          logger.LogInformation( "http pipeline injected." );

        builder.Run( application );
      } );
    }



    private class PipelineStartupFilter : IStartupFilter
    {
      public PipelineStartupFilter()
      {
      }

      public Action<IApplicationBuilder> Configure( Action<IApplicationBuilder> next )
      {

        if ( _handler == null )
          throw new InvalidOperationException( "must invoke Run method when pipeline is completed." );


        return builder =>
        {
          builder.Run( _handler );
          next( builder );
        };
      }

      private RequestDelegate _handler;

      internal void SetApplication( RequestDelegate handler )
      {
        _handler = handler;
      }
    }







    /// <summary>
    /// forward request to downstream pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <returns>pipeline</returns>
    public static IHttpPipeline Forward( this IApplicationBuilder application )
    {
      return application.UsePipeline( new AspNetCoreForwardProxy( ForwardProxyMode.None ) );
    }

    /// <summary>
    /// insert a forward proxy to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="transmit">transmit headers behavior</param>
    /// <param name="proxyMode">build proxy headers behavior</param>
    /// <returns>pipeline</returns>
    public static IHttpPipeline ForwardProxy( this IApplicationBuilder application, ForwardProxyMode proxyMode = ForwardProxyMode.Legacy )
    {
      return application.UsePipeline( new AspNetCoreForwardProxy( proxyMode ) );
    }



    public static void Run( this IHttpPipeline pipeline )
    {
      //TODO get emitter from depencency injection.
      pipeline.Emit( new HttpPipelineEmitter() );
    }







    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="configure">处理管线构建程序</param>
    public static void Run( this IApplicationBuilder application, Func<IHttpPipeline, IHttpPipelineHandler> configure )
    {
      var pipeline = configure( Ivony.Http.Pipeline.HttpPipeline.Blank );
      application.Run( pipeline );
    }



    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="configure">处理管线构建程序</param>
    public static void Run( this IApplicationBuilder application, Func<IHttpPipeline, IHttpPipeline> configure )
    {
      var pipeline = configure( Ivony.Http.Pipeline.HttpPipeline.Blank );
      application.Run( pipeline.Emit( application.ApplicationServices.GetService<IHttpPipelineEmitter>() ) );
    }


    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="pipeline">HTTP 请求处理管线</param>
    public static void Run( this IApplicationBuilder application, IHttpPipelineHandler pipeline )
    {
      var accessPoint = application.ApplicationServices.GetService<IHttpPipelineAccessPoint<RequestDelegate>>();
      if ( accessPoint != null )
      {
        application.Run( accessPoint.Combine( pipeline ) );
        return;
      }

      throw new InvalidOperationException( "asp.net core access point service is not register yet." );
    }

  }
}
