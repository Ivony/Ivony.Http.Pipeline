using System.Net.Http;

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
      return (HttpPipelineRouteData) request.Properties[HttpPipelineRouter.RouteDataKey];
    }

  }
}
