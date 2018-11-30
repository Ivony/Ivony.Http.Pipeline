using System.Net.Http;

namespace Ivony.Http.Pipeline
{
  public abstract class HttpPipelineRouteRule : IHttpPipeline
  {

    public abstract bool CheckReqeust( HttpRequestMessage request );

    public abstract HttpPipelineHandler Pipe( HttpPipelineHandler handler );
  }
}