using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ivony.Http.Pipeline;
using Ivony.Http.Pipeline.Routes;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{
  public static class ExtensionsRewrite
  {


    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="template">rewrite template.</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( this IHttpPipeline pipeline, string template )
    {
      return Rewrite( pipeline, new RouteRequestTemplate( template ) );
    }

    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="template">rewrite template.</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( IHttpPipeline pipeline, RouteRequestTemplate template )
    {
      return pipeline.JoinPipeline( handler => request =>
      {
        request = template.RewriteRequest( request, new Dictionary<string, string>() );
        return handler( request );
      } );
    }



    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstream">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( this IHttpPipeline pipeline, string upstream, string downstream )
    {
      return Rewrite( pipeline, new RouteRequestTemplate( upstream ), new RouteRequestTemplate( downstream ) );
    }

    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstream">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( IHttpPipeline pipeline, RouteRequestTemplate upstream, RouteRequestTemplate downstream )
    {
      var rewriter = new RouteRewriteRule( new[] { upstream }, downstream );

      return pipeline.JoinPipeline( handler => request =>
      {
        request = rewriter.Rewrite( request );
        return handler( request );
      } );
    }



    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstreams">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( this IHttpPipeline pipeline, params string[] templates )
    {
      var upstreams = templates.Take( templates.Length - 1 ).Select( t => new RouteRequestTemplate( t ) ).ToArray();
      var downstream = new RouteRequestTemplate( templates.Last() );

      return Rewrite( pipeline, upstreams, downstream );
    }

    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstreams">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( IHttpPipeline pipeline, RouteRequestTemplate[] upstreams, RouteRequestTemplate downstream )
    {
      var rewriter = new RouteRewriteRule( upstreams, downstream );

      return pipeline.JoinPipeline( handler => request =>
      {
        request = rewriter.Rewrite( request );
        return handler( request );
      } );
    }




    /// <summary>
    /// 重写请求的 Host 属性
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="host">要重写的主机头</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline RewriteHost( this IHttpPipeline pipeline, string host )
    {
      return RewriteHost( pipeline, new HostString( host ) );
    }


    /// <summary>
    /// 重写请求的 Host 属性
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="host">要重写的主机头</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline RewriteHost( this IHttpPipeline pipeline, HostString host )
    {
      return pipeline.JoinPipeline( new HttpRequestFilter( request =>
      {
        var builder = new UriBuilder( request.RequestUri );

        if ( host.HasValue )
        {
          builder.Host = host.Host;
          builder.Port = host.Port ?? GetDefaultPort( builder.Scheme ) ?? builder.Port;
        }


        request.RequestUri = builder.Uri;
        return request;
      } ) );
    }

    private static int? GetDefaultPort( string scheme )
    {
      if ( scheme == Uri.UriSchemeHttp )
        return 80;

      else
        return null;
    }
  }
}
