using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Ivony.Http.Pipeline.Routes
{
  /// <summary>
  /// route and rewrite request rule
  /// </summary>
  public class RouteRewriteRule : IHttpPipelineRewriteRule, IHttpPipelineRouteRule, IHttpPipeline
  {

    /// <summary>
    /// upstream templates, match the request and extract route values
    /// </summary>
    public RewriteRequestTemplate[] Upstreams { get; }

    /// <summary>
    /// downstream template, rewrite thr request with route values
    /// </summary>
    public RewriteRequestTemplate Downstream { get; }



    /// <summary>
    /// create RouteRewriteRule instance
    /// </summary>
    /// <param name="upstreamTemplates">upstream templates</param>
    /// <param name="downstreamTemplate">downstream template</param>
    public RouteRewriteRule( IReadOnlyList<RewriteRequestTemplate> upstreamTemplates, RewriteRequestTemplate downstreamTemplate )
    {
      Upstreams = upstreamTemplates.ToArray();
      Downstream = downstreamTemplate;
    }


    public RouteRewriteRule( string upstream, string downstream ) : this( new[] { new RewriteRequestTemplate( upstream ) }, new RewriteRequestTemplate( downstream ) ) { }



    /// <summary>
    /// 产生路由值
    /// </summary>
    /// <param name="requestData"></param>
    /// <returns></returns>
    public IReadOnlyDictionary<string, string> Match( RouteRequestData requestData )
    {
      foreach ( var item in Upstreams )
      {
        var values = item.GetRouteValues( requestData );
        if ( values != null )
          return values;
      }

      return null;
    }


    /// <summary>
    /// 重写请求
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public HttpRequestMessage Rewrite( HttpRequestMessage request )
    {
      var routeValues = GetRouteValues( request );
      if ( routeValues == null )
        return request;

      return Downstream.RewriteRequest( request, routeValues );
    }

    private IReadOnlyDictionary<string, string> GetRouteValues( HttpRequestMessage request )
    {
      var routeData = request.GetRouteData();
      if ( routeData?.RouteRule == this )
        return routeData.Values;

      else
        return Match( new RouteRequestData( request ) );
    }

    public HttpPipelineHandler Join( HttpPipelineHandler handler )
    {
      return request =>
      {
        request = Rewrite( request );
        return handler( request );
      };
    }
  }
}
