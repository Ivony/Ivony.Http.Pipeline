using Microsoft.AspNetCore.Builder;

namespace Ivony.Http.Pipeline
{
  public sealed class HttpPipelineBuilder : IHttpPipelineMiddleware
  {


    private IHttpPipeline _pipeline;

    public IHttpPipeline Build()
    {
      return _pipeline;
    }


    public IHttpPipeline Pipe( IHttpPipeline pipeline )
    {
      return _pipeline = pipeline;
    }
  }
}