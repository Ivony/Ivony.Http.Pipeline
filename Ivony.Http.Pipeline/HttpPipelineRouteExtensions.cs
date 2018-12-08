using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline
{
  public static class HttpPipelineRouteExtensions
  {

    /// <summary>
    /// get the route data, if pipeline cross a router.
    /// </summary>
    /// <param name="request"></param>
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
      return pipeline.Pipe( new HttpPipelineRouter( rules ) );
    }

    /// <summary>
    /// use a router.
    /// </summary>
    /// <param name="pipeline">pipeline to use router</param>
    /// <param name="rules">route rules</param>
    /// <returns>pipeline with router</returns>
    public static IHttpPipeline UseRouter( this IHttpPipeline pipeline, params (string upstreamTemplate, string downstreamTemplate)[] rules )
    {
      return UseRouter( pipeline, rules.Select( r => new RouteRewriteRule( r.upstreamTemplate, r.downstreamTemplate ) ).ToArray() );
    }

    /// <summary>
    /// use a router.
    /// </summary>
    /// <param name="pipeline">pipeline to use router</param>
    /// <param name="rules">route rules</param>
    /// <returns>pipeline with router</returns>
    public static IHttpPipeline UseRouter( this IHttpPipeline pipeline, params (string[] upstreamTemplates, string downstreamTemplate)[] rules )
    {
      return UseRouter( pipeline, rules.Select( r => new RouteRewriteRule( r.upstreamTemplates.Select( t => new RouteRequestTemplate( t ) ).ToArray(), new RouteRequestTemplate( r.downstreamTemplate ) ) ).ToArray() );
    }


    /// <summary>
    /// use a router.
    /// </summary>
    /// <param name="pipeline">pipeline to use router</param>
    /// <param name="rules">route rules</param>
    /// <returns>pipeline with router</returns>
    public static IHttpPipeline UseRouter( this IHttpPipeline pipeline, Action<IRouteRulesBuilder> configure )
    {
      var builder = new IRouteRulesBuilder();
      configure( builder );
      return UseRouter( pipeline, builder.GetRules() );
    }




    public static RouteRewriteRuleBuilder Match( this IRouteRulesBuilder builder, string upstreamTemplate )
    {
      return new RouteRewriteRuleBuilder( builder ).Match( upstreamTemplate );
    }


  }
}
