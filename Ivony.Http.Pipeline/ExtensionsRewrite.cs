using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ivony.Http.Pipeline.Routes;

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
      => pipeline.Join( RewriteRule.Create( template ) );

    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="template">rewrite template.</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( IHttpPipeline pipeline, RewriteRequestTemplate template )
      => pipeline.Join( RewriteRule.Create( template ) );




    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstream">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( this IHttpPipeline pipeline, string upstream, string downstream )
      => pipeline.Join( RewriteRule.Create( upstream, downstream ) );



    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstream">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( IHttpPipeline pipeline, RewriteRequestTemplate upstream, RewriteRequestTemplate downstream )
      => pipeline.Join( RewriteRule.Create( upstream, downstream ) );




    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstreams">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( this IHttpPipeline pipeline, params string[] templates )
      => pipeline.Join( RewriteRule.Create( templates ) );

    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstreams">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( IHttpPipeline pipeline, RewriteRequestTemplate[] upstreams, RewriteRequestTemplate downstream )
      => pipeline.Join( RewriteRule.Create( upstreams, downstream ) );



    /// <summary>
    /// rewrite request host
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="host">host string to rewrite</param>
    /// <returns>pipeline with host rewrite</returns>
    public static IHttpPipeline RewriteHost( this IHttpPipeline pipeline, string host )
      => Rewrite( pipeline, "/{path*}", "//" + host + "/{path}" );

  }
}
