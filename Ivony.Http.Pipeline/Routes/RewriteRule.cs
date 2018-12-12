using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Routes
{
  /// <summary>
  /// route and rewrite request rule
  /// </summary>
  public class RewriteRule : IHttpPipelineRewriteRule, IHttpPipelineRouteRule, IHttpPipeline
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
    public RewriteRule( IReadOnlyList<RewriteRequestTemplate> upstreamTemplates, RewriteRequestTemplate downstreamTemplate )
    {
      Upstreams = upstreamTemplates.ToArray();
      Downstream = downstreamTemplate;
    }


    public RewriteRule( string upstream, string downstream ) : this( new[] { new RewriteRequestTemplate( upstream ) }, new RewriteRequestTemplate( downstream ) ) { }



    /// <summary>
    /// 产生路由值
    /// </summary>
    /// <param name="requestData"></param>
    /// <returns></returns>
    public IReadOnlyDictionary<string, string> Match( RouteRequestData requestData )
    {

      if ( Upstreams.Any() == false )
        return new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );

      foreach ( var item in Upstreams )
      {
        var values = item.GetRouteValues( requestData );
        if ( values != null )
          return values;
      }

      return null;
    }


    /// <summary>
    /// rewrite request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public HttpRequestMessage Rewrite( HttpRequestMessage request )
    {
      var routeValues = TryGetRouteValues( request );
      if ( routeValues == null )
        return request;

      return Downstream.RewriteRequest( request, routeValues );
    }

    private IReadOnlyDictionary<string, string> TryGetRouteValues( HttpRequestMessage request )
    {
      var routeData = request.GetRouteData();
      if ( routeData?.RouteRule == this )
        return routeData.Values;

      else if ( Upstreams.Any() )
        return Match( new RouteRequestData( request ) );

      else
        return new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );
    }

    public IHttpPipelineHandler Join( IHttpPipelineHandler handler )
    {
      return new RewritePipelineHandler( this, handler );
    }

    private class RewritePipelineHandler : IHttpPipelineHandler
    {
      private readonly IHttpPipelineRewriteRule _rewriter;
      private readonly IHttpPipelineHandler _handler;

      public RewritePipelineHandler( IHttpPipelineRewriteRule rewriter, IHttpPipelineHandler handler )
      {
        _rewriter = rewriter;
        this._handler = handler;
      }

      public ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
      {
        request = _rewriter.Rewrite( request );
        return _handler.ProcessRequest( request );
      }
    }

  }
}
