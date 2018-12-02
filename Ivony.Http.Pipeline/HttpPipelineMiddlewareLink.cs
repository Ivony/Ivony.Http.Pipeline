namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 链接两个管线中间件的链接器
  /// </summary>
  internal sealed class HttpPipelineMiddlewareLink : IHttpPipeline
  {
    private IHttpPipeline pipeline;
    private IHttpPipeline next;

    public HttpPipelineMiddlewareLink( IHttpPipeline pipeline, IHttpPipeline nextPipeline )
    {
      this.pipeline = pipeline;
      this.next = nextPipeline;
    }

    HttpPipelineHandler IHttpPipeline.Pipe( HttpPipelineHandler downstream )
    {
      return this.pipeline.Pipe( next.Pipe( downstream ) );
    }
  }
}