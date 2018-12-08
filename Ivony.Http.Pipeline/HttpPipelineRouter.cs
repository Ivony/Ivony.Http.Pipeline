using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ivony.Http.Pipeline.Routes;
using Microsoft.AspNetCore.Http;

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
    /// <param name="rules">route rules</param>
    public HttpPipelineRouter( params IHttpPipelineRouteRule[] rules )
    {
      Rules = rules ?? throw new ArgumentNullException( nameof( rules ) );
    }


    /// <summary>
    /// get the route rules.
    /// </summary>
    public IHttpPipelineRouteRule[] Rules { get; }

    internal static readonly string RouteDataKey = "__RouteData";


    /// <summary>
    /// implement Pipe method to route request.
    /// </summary>
    /// <param name="downstream">downstream pipeline handler</param>
    /// <returns>a new pipeline handler with route rules.</returns>
    public HttpPipelineHandler Pipe( HttpPipelineHandler downstream )
    {
      var _rules = Rules.Select( rule => (Func<HttpRequestMessage, HttpPipelineHandler>) (request =>
      {
        var values = rule.Match( new RouteRequestData( request ) );
        if ( values == null )
          return null;

        CreateRouteData( rule, request, values );

        if ( rule is IHttpPipeline pipeline )
          return pipeline.Pipe( downstream );

        return downstream;

      }) ).ToArray();


      return new HttpPipelineConditionDistributer( _rules, HandleExcept ).AsHandler();
    }


    private void CreateRouteData( IHttpPipelineRouteRule rule, HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> values )
    {
      var routeData = new HttpPipelineRouteData( request.GetRouteData(), this, rule, values );
      request.Properties[RouteDataKey] = routeData;
    }


    /// <summary>
    /// handle request when no rule matched.
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <returns>response</returns>
    protected virtual Task<HttpResponseMessage> HandleExcept( HttpRequestMessage request )
    {
      return Task.FromResult( new HttpResponseMessage( System.Net.HttpStatusCode.NotFound )
      {
        Content = new ByteArrayContent( new byte[0] )
      } );

    }
  }
}
