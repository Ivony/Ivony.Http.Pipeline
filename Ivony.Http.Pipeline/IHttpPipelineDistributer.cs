using System.Net.Http;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// define a HTTP pipeline distributer, it's distribute the HTTP request to number of pipeline to handle it.
  /// </summary>
  public interface IHttpPipelineDistributer
  {

    /// <summary>
    /// distribute HTTP request.
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <returns>a handler to handle this request</returns>
    IHttpPipelineHandler Distribute( HttpRequestMessage request );
  }
}