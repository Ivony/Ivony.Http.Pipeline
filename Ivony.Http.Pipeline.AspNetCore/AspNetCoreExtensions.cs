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
    /// forward request to downstream pipeline and add forward proxy headers
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
    /// run asp.net core site with pipeline.
    /// </summary>
    /// <param name="builder">asp.net core application builder</param>
    /// <param name="configure">HTTP pipeline configure</param>
    public static void RunPipeline( this IApplicationBuilder builder, Func<IHttpPipeline, IHttpPipelineHandler> configure )
    {
      var pipeline = configure( Ivony.Http.Pipeline.HttpPipeline.Blank );
      builder.RunPipeline( pipeline );
    }



    /// <summary>
    /// run asp.net core site with pipeline.
    /// </summary>
    /// <param name="builder">asp.net core application builder</param>
    /// <param name="configure">HTTP pipeline configure</param>
    public static void RunPipeline( this IApplicationBuilder builder, Func<IHttpPipeline, IHttpPipeline> configure )
    {
      var pipeline = configure( Ivony.Http.Pipeline.HttpPipeline.Blank );
      builder.RunPipeline( pipeline.Emit( builder.ApplicationServices.GetService<IHttpPipelineEmitter>() ) );
    }


    /// <summary>
    /// run asp.net core site with pipeline.
    /// </summary>
    /// <param name="builder">asp.net core application builder</param>
    /// <param name="pipeline">HTTP pipeline</param>
    public static void RunPipeline( this IApplicationBuilder builder, IHttpPipelineHandler pipeline )
    {
      var accessPoint = builder.ApplicationServices.GetRequiredService<IHttpPipelineAccessPoint<RequestDelegate>>();
      builder.Run( accessPoint.Combine( pipeline ) );
    }


    /// <summary>
    /// run asp.net core site with pipeline.
    /// </summary>
    /// <param name="builder">asp.net core application builder</param>
    /// <param name="configure">HTTP pipeline configure</param>
    public static void RunPipeline( this IApplicationBuilder builder, Action<AspNetCorePipelineBuilder> configure )
    {
      var pipelineBuilder = new AspNetCorePipelineBuilder();
      configure( pipelineBuilder );
      if ( pipelineBuilder.Application == null )
        throw new InvalidOperationException( "HTTP pipeline is not completed yet." );

      else
        builder.Run( pipelineBuilder.Application );

    }

  }
}
