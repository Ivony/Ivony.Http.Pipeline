using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Ivony.Http.Pipeline;
using Ivony.Http.Pipeline.Handlers;
using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// privide extension methods of route
  /// </summary>
  public static class ExtensionsRoute
  {

    /// <summary>
    /// get the route data, if pipeline cross a router.
    /// </summary>
    /// <param name="request">request message</param>
    /// <returns></returns>
    public static HttpPipelineRouteData GetRouteData( this HttpRequestMessage request )
    {
      if ( request.Properties.TryGetValue( HttpPipelineRouter.RouteDataKey, out var data ) )
        return data as HttpPipelineRouteData;

      else
        return null;
    }


    /// <summary>
    /// use a router.
    /// </summary>
    /// <param name="pipeline">pipeline to use router</param>
    /// <param name="rules">route rules</param>
    /// <returns>pipeline with router</returns>
    public static IHttpPipeline UseRouter( this IHttpPipeline pipeline, params IHttpPipelineRouteRule[] rules )
    {
      return UseRouter( pipeline, new HttpNotFound(), rules );
    }


    /// <summary>
    /// use a router.
    /// </summary>
    /// <param name="pipeline">pipeline to use router</param>
    /// <param name="otherwise">handler that handle request when no rule matches.</param>
    /// <param name="rules">route rules</param>
    /// <returns>pipeline with router</returns>
    public static IHttpPipeline UseRouter( this IHttpPipeline pipeline, IHttpPipelineHandler otherwise, params IHttpPipelineRouteRule[] rules )
    {
      return pipeline.Join( new HttpPipelineRouter( otherwise, rules ) );
    }

    /// <summary>
    /// use a router.
    /// </summary>
    /// <param name="pipeline">pipeline to use router</param>
    /// <param name="rules">route rules</param>
    /// <returns>pipeline with router</returns>
    public static IHttpPipeline UseRouter( this IHttpPipeline pipeline, params (string upstreamTemplate, string downstreamTemplate)[] rules )
    {
      return UseRouter( pipeline, rules.Select( r => RewriteRule.Create( r.upstreamTemplate, r.downstreamTemplate ) ).ToArray() );
    }

    /// <summary>
    /// use a router.
    /// </summary>
    /// <param name="pipeline">pipeline to use router</param>
    /// <param name="rules">route rules</param>
    /// <returns>pipeline with router</returns>
    public static IHttpPipeline UseRouter( this IHttpPipeline pipeline, params (string[] upstreamTemplates, string downstreamTemplate)[] rules )
    {
      return UseRouter( pipeline, rules.Select( r => new RewriteRule( r.upstreamTemplates.Select( t => new RewriteRequestTemplate( t ) ).ToArray(), new RewriteRequestTemplate( r.downstreamTemplate ) ) ).ToArray() );
    }


    /// <summary>
    /// use a router.
    /// </summary>
    /// <param name="pipeline">pipeline to use router</param>
    /// <param name="configure">method to configure route rules</param>
    /// <returns>pipeline with router</returns>
    public static IHttpPipeline UseRouter( this IHttpPipeline pipeline, Action<IRouteRulesBuilder> configure )
    {
      var builder = new RouteRulesBuilder();
      configure( builder );
      return UseRouter( pipeline, builder.GetOtherwiseHandler() ?? new HttpNotFound(), builder.GetRules() );
    }




    /// <summary>
    /// match one or more route rule
    /// </summary>
    /// <param name="builder">route rule builder</param>
    /// <param name="upstream">upstream template to match</param>
    /// <returns>a RouteRewriteRuleBuilder instance, use it to add more match rules, or rewrite them.</returns>
    public static RouteRewriteRuleBuilder Match( this IRouteRulesBuilder builder, string upstream )
    {
      return new RouteRewriteRuleBuilder( builder ).Match( upstream );
    }


    /// <summary>
    /// rewrite matched request
    /// </summary>
    /// <param name="builder">route rule builder</param>
    /// <param name="upstream">upstream template to match</param>
    /// <param name="downstrem">downstream template to rewrite</param>
    /// <returns>a IRouteRulesBuilder instance, use it to add more match or rewrite rules.</returns>
    public static IRouteRulesBuilder Rewrite( this IRouteRulesBuilder builder, string upstream, string downstrem )
    {
      builder.AddRule( RewriteRule.Create( upstream, downstrem ) );
      return builder;
    }


  }
}
