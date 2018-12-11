using System;

using Ivony.Http.Pipeline;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class AspNetCoreExtensions
  {


    public static IServiceCollection AddHttpPipeline( this IServiceCollection services )
    {
      services.AddSingleton<IHttpPipelineEmitter, HttpPipelineEmitter>();
      services.AddSingleton<IHttpPipelineAccessPoint<Func<RequestDelegate, RequestDelegate>>, AspNetCoreCombinator>();

      return services;
    }


    /// <summary>
    /// 使用 HTTP 请求处理管线
    /// </summary>
    /// <param name="application">ASP.NET Core 应用构建器</param>
    /// <param name="configure">处理管线构建程序</param>
    public static IHttpPipeline UsePipeline( this IApplicationBuilder application, IHttpPipelineAccessPoint<Func<RequestDelegate, RequestDelegate>> accessPoint )
    {
      return new HttpPipeline( application, accessPoint );
    }


    private class HttpPipeline : IHttpPipeline
    {
      private readonly IApplicationBuilder _builder;
      private readonly IHttpPipelineAccessPoint<Func<RequestDelegate, RequestDelegate>> _accessPoint;

      public HttpPipeline( IApplicationBuilder builder, IHttpPipelineAccessPoint<Func<RequestDelegate, RequestDelegate>> accessPoint )
      {
        _builder = builder ?? throw new ArgumentNullException( nameof( builder ) );
        _accessPoint = accessPoint ?? throw new ArgumentNullException( nameof( accessPoint ) );
      }

      public HttpPipelineHandler Join( HttpPipelineHandler downstream )
      {
        _builder.Use( _accessPoint.Combine( downstream ) );
        var logger = _builder.ApplicationServices.GetService<ILogger<AspNetCoreCombinator>>();
        if ( logger != null )
          logger.LogInformation( "http pipeline injected." );

        return null;
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

      {
        var accessPoint = application.ApplicationServices.GetService<IHttpPipelineAccessPoint<Func<RequestDelegate, RequestDelegate>>>();
        if ( accessPoint != null )
        {
          application.Use( accessPoint.Combine( pipeline ) );
          return;
        }
      }

      {
        var accessPoint = application.ApplicationServices.GetService<IHttpPipelineAccessPoint<RequestDelegate>>();
        if ( accessPoint != null )
        {
          application.Use( continuation => accessPoint.Combine( pipeline ) );
          return;
        }
      }

      throw new InvalidOperationException( "asp.net core access point service is not register yet." );
    }

  }
}
