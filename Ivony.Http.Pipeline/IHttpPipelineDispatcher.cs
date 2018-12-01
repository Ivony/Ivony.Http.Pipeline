using System.Net.Http;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// define a HTTP pipeline dispatcher, it's dispatch the HTTP request to diffrent pipeline to handle it.
  /// </summary>
  public interface IHttpPipelineDispatcher
  {

    /// <summary>
    /// dispatch HTTP request.
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <returns>a handler to handle this request</returns>
    HttpPipelineHandler Dispatch( HttpRequestMessage request );
  }
}