using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// implement a http piieline route service.
  /// </summary>
  public class HttpPipelineRouter : IHttpPipeline
  {


    /// <summary>
    /// create a HttpPipelineRouteService instance
    /// </summary>
    /// <param name="otherwiseHandler">handler that handle request when there is no rule match</param>
    /// <param name="rules">route rules</param>
    public HttpPipelineRouter( IHttpPipelineHandler otherwiseHandler, params IHttpPipelineRouteRule[] rules )
    {
      OtherwiseHandler = otherwiseHandler ?? throw new ArgumentNullException( nameof( otherwiseHandler ) );
      Rules = rules ?? throw new ArgumentNullException( nameof( rules ) );
    }


    /// <summary>
    /// get the route rules.
    /// </summary>
    public IHttpPipelineRouteRule[] Rules { get; }

    internal static readonly string RouteDataKey = "__RouteData";


    /// <summary>
    /// implement Join method to route request.
    /// </summary>
    /// <param name="downstream">downstream pipeline handler</param>
    /// <returns>a new pipeline handler with route rules.</returns>
    public IHttpPipelineHandler Join( IHttpPipelineHandler downstream )
    {
      var _rules = Rules.Select( rule => (Func<HttpRequestMessage, IHttpPipelineHandler>) (request =>
      {
        var values = rule.Match( new RouteRequestData( request ) );
        if ( values == null )
          return null;

        CreateRouteData( rule, request, values );

        if ( rule is IHttpPipeline pipeline )
          return pipeline.Join( downstream );

        return downstream;

      }) ).ToArray();


      return new HttpPipelineConditionDistributer( _rules, OtherwiseHandler ).AsHandler();
    }



    public IHttpPipelineHandler OtherwiseHandler { get; }


    private void CreateRouteData( IHttpPipelineRouteRule rule, HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> values )
    {
      var routeData = new HttpPipelineRouteData( request.GetRouteData(), this, rule, values );
      request.Properties[RouteDataKey] = routeData;
    }



    public class RouteNotMatchHandler : IHttpPipelineHandler
    {
      /// <summary>
      /// handle request when no rule matched.
      /// </summary>
      /// <param name="request">HTTP request message</param>
      /// <returns>response</returns>
      public ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
      {
        return new ValueTask<HttpResponseMessage>( new HttpResponseMessage( System.Net.HttpStatusCode.NotFound )
        {
          Content = new ByteArrayContent( new byte[0] )
        } );
      }
    }
  }
}
