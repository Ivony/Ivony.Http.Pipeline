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
    {
      return Rewrite( pipeline, new RewriteRequestTemplate( template ) );
    }

    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="template">rewrite template.</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( IHttpPipeline pipeline, RewriteRequestTemplate template )
    {
      var rewriter = RewriteRule.Create( template );
      return pipeline.JoinPipeline( rewriter );
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
      var rewriter = RewriteRule.Create( upstream, downstream );
      return pipeline.JoinPipeline( rewriter );
    }

    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstream">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( IHttpPipeline pipeline, RewriteRequestTemplate upstream, RewriteRequestTemplate downstream )
    {
      var rewriter = RewriteRule.Create( upstream, downstream );
      return pipeline.JoinPipeline( rewriter );
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

      var rewriter = RewriteRule.Create( templates );
      return pipeline.JoinPipeline( rewriter );
    }

    /// <summary>
    /// insert a rewrite rule to pipeline
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="upstreams">upstream rule, or called route rule.</param>
    /// <param name="downstream">downstream rule, or called rewrite rule</param>
    /// <returns>pipeline with rewrite rule</returns>
    public static IHttpPipeline Rewrite( IHttpPipeline pipeline, RewriteRequestTemplate[] upstreams, RewriteRequestTemplate downstream )
    {
      var rewriter = new RewriteRule( upstreams, downstream );

      return pipeline.JoinPipeline( rewriter );
    }



    /// <summary>
    /// 重写请求的 Host 属性
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="host">要重写的主机头</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline RewriteHost( this IHttpPipeline pipeline, string host )
    {
      return Rewrite( pipeline, "/{path*}", "//" + host + "/{path}" );
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
