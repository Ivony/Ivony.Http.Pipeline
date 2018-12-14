using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Ivony.Http.Pipeline;
using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline
{
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
      return pipeline.Join( new HttpPipelineRouter( rules ) );
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
    /// <param name="rules">route rules</param>
    /// <returns>pipeline with router</returns>
    public static IHttpPipeline UseRouter( this IHttpPipeline pipeline, Action<IRouteRulesBuilder> configure )
    {
      var builder = new RouteRulesBuilder();
      configure( builder );
      return UseRouter( pipeline, builder.GetRules() );
    }




    public static RouteRewriteRuleBuilder Match( this IRouteRulesBuilder builder, string upstreamTemplate )
    {
      return new RouteRewriteRuleBuilder( builder ).Match( upstreamTemplate );
    }


  }
}
