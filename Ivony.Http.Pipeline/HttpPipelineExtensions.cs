using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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


    public static IHttpPipelineMiddleware Pipe( this IHttpPipelineMiddleware middleware, IHttpPipelineMiddleware nextMiddleware )
    {
      return new HttpPipelineMiddlewareLink( middleware, nextMiddleware );
    }


    public static IHttpPipeline UseLoadBalancer( this IHttpPipelineMiddleware middleware, Action<HttpPipelineDispatcherBuilder> configure )
    {
      var builder = new HttpPipelineDispatcherBuilder();
      configure( builder );
      return middleware.Pipe( builder.Build() );
    }

    public static IHttpPipeline UseLoadBalancer( this IHttpPipelineMiddleware middleware, params Func<IHttpPipelineMiddleware, IHttpPipeline>[] pipelines )
    {
      return middleware.Pipe( new HttpPipelineDispatcher( pipelines.Select( func => func( new HttpPipelineBuilder() ) ).ToArray() ) );
    }



    public static IHttpPipelineMiddleware RewriteHost( this IHttpPipelineMiddleware middleware, string host )
    {
      return middleware.Pipe( new HttpRequestFilter( request =>
      {
        var url = new UriBuilder( request.RequestUri );
        url.Host = host;
        request.RequestUri = url.Uri;
        return request;
      } ) );
    }

    public static IHttpPipeline Emit( this IHttpPipelineMiddleware middleware )
    {
      return middleware.Pipe( new HttpEmitter() );
    }



    public static IApplicationBuilder UsePipeline( this IApplicationBuilder application, Action<HttpPipelineBuilder> configure )
    {
      var builder = new HttpPipelineBuilder();
      configure( builder );
      application.UsePipeline( builder.Build() );

      return application;
    }


    public static void UsePipeline( this IApplicationBuilder application, IHttpPipeline pipeline )
    {
      application.Use( continuation => async context =>
      {
        var request = CreateRequest( context );

        var response = await pipeline.ProcessRequest( request );

        ApplyResponse( context, response );

      } );
    }

    private static void ApplyResponse( HttpContext context, HttpResponseMessage response )
    {
      throw new NotImplementedException();
    }

    private static HttpRequestMessage CreateRequest( HttpContext context )
    {
      throw new NotImplementedException();
    }





  }
}
