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



    public static RewriteRule Create( string template )
    {
      return Create( new RewriteRequestTemplate( template ) );
    }

    private static RewriteRule Create( RewriteRequestTemplate template )
    {
      return new RewriteRule( new RewriteRequestTemplate[0], template );
    }



    public static RewriteRule Create( string upstream, string downstream )
    {
      return Create( new RewriteRequestTemplate( upstream ), new RewriteRequestTemplate( downstream ) );
    }

    public static RewriteRule Create( RewriteRequestTemplate upstream, RewriteRequestTemplate downstream )
    {
      return new RewriteRule( new[] { upstream }, downstream );
    }



    public static RewriteRule Create( string[] upstreams, string downstream )
    {
      return Create( upstreams.Select( item => new RewriteRequestTemplate( item ) ).ToArray(), new RewriteRequestTemplate( downstream ) );
    }

    public static RewriteRule Create( RewriteRequestTemplate[] upstreams, RewriteRequestTemplate downstream )
    {
      return new RewriteRule( upstreams, downstream );
    }



    public static RewriteRule Create( params string[] templates )
    {
      if ( templates.Length == 0 )
        throw new ArgumentOutOfRangeException( nameof( templates ) );


      return Create( templates.Select( item => new RewriteRequestTemplate( item ) ).ToArray() );
    }


    public static RewriteRule Create( params RewriteRequestTemplate[] templates )
    {
      if ( templates.Length == 0 )
        throw new ArgumentOutOfRangeException( nameof( templates ) );

      else if ( templates.Length == 1 )
        return Create( templates[0] );



      var upstreams = templates.Take( templates.Length - 1 ).ToArray();
      var downstream = templates.Last();

      return new RewriteRule( upstreams, downstream );
    }



    /// <summary>
    /// 产生路由值
    /// </summary>
    /// <param name="requestData"></param>
    /// <returns></returns>
    public IReadOnlyDictionary<string, string> Match( RouteRequestData requestData )
    {

      if ( Upstreams.Any() == false )                //if has no upstream templates, it's match any request.
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
