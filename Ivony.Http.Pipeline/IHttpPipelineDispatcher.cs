using System.Net.Http;

namespace Ivony.Http.Pipeline
{
  public interface IHttpPipelineDispatcher
  {
    HttpPipelineHandler Dispatch( HttpRequestMessage request );
  }
}