using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  /// <summary>
  /// route and rewrite request rule
  /// </summary>
  public class RouteRewriteRule : IHttpPipelineRewriteRule, IHttpPipeline
  {

    /// <summary>
    /// 上游模板
    /// </summary>
    public RouteRequestTemplate[] Upstreams { get; }

    /// <summary>
    /// 下游模板
    /// </summary>
    public RouteRequestTemplate Downstream { get; }



    public RouteRewriteRule( IReadOnlyList<RouteRequestTemplate> upstreamTemplates, RouteRequestTemplate downstreamTemplate )
    {
      Upstreams = upstreamTemplates.ToArray();
      Downstream = downstreamTemplate;
    }


    public RouteRewriteRule( string upstream, string downstream ) : this( new[] { new RouteRequestTemplate( upstream ) }, new RouteRequestTemplate( downstream ) ) { }



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

    public HttpPipelineHandler Pipe( HttpPipelineHandler handler )
    {
      return request =>
      {
        request = Rewrite( request );
        return handler( request );
      };
    }
  }
}
