using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  public class RouteRewriteRule : IHttpPipelineRouteRule, IHttpPipeline
  {

    /// <summary>
    /// 上游模板
    /// </summary>
    public RouteRequestTemplate Upstream { get; }

    /// <summary>
    /// 下游模板
    /// </summary>
    public RouteRequestTemplate Downstream { get; }


    public RouteRewriteRule( string upstreamTemplate, string downstreamTemplate )
    {
      Upstream = new RouteRequestTemplate( upstreamTemplate );
      Downstream = new RouteRequestTemplate( downstreamTemplate );
    }


    /// <summary>
    /// 产生路由值
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public IDictionary<string, string> Route( HttpRequestMessage request )
    {
      return Upstream.GetRouteValues( request );
    }

    public HttpPipelineHandler Pipe( HttpPipelineHandler handler )
    {
      return request =>
      {
        request = Downstream.RewriteRequest( request, request.GetRouteData().Values );
        return handler( request );
      };
    }


  }
}
