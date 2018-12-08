
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// a container of route values and route pipeline.
  /// </summary>
  public sealed class HttpPipelineRouteData
  {
    /// <summary>
    /// create HttpPipelineRouteData instance.
    /// </summary>
    /// <param name="values">route values.</param>
    /// <param name="pipeline">route pipeline.</param>
    internal HttpPipelineRouteData( HttpPipelineRouteData overridedRouteData, HttpPipelineRouter router, IHttpPipelineRouteRule rule, IEnumerable<KeyValuePair<string, string>> values )
    {
      OverridedRouteData = overridedRouteData;
      Router = router ?? throw new ArgumentNullException( nameof( router ) );
      RouteRule = rule ?? throw new ArgumentNullException( nameof( rule ) );

      if ( values == null )
        throw new ArgumentNullException( nameof( values ) );
      Values = new ReadOnlyDictionary<string, string>( new Dictionary<string, string>( values, StringComparer.OrdinalIgnoreCase ) );
    }


    /// <summary>
    /// thr router service that process this route data.
    /// </summary>
    public HttpPipelineRouter Router { get; }


    /// <summary>
    /// route rule that create the route data.
    /// </summary>
    public IHttpPipelineRouteRule RouteRule { get; }


    /// <summary>
    /// get route values.
    /// </summary>
    public IReadOnlyDictionary<string, string> Values { get; }


    /// <summary>
    /// the route data that overrided by this route data.
    /// </summary>
    public HttpPipelineRouteData OverridedRouteData { get; }

  }
}