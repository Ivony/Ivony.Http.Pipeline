namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 链接两个管线中间件的链接器
  /// </summary>
  internal sealed class HttpPipelineMiddlewareLink : IHttpPipeline
  {
    private IHttpPipeline _upstream;
    private IHttpPipeline _downstream;

    public HttpPipelineMiddlewareLink( IHttpPipeline upstreamPipeline, IHttpPipeline downstreamPipeline )
    {
      _upstream = upstreamPipeline;
      _downstream = downstreamPipeline;
    }

    HttpPipelineHandler IHttpPipeline.Pipe( HttpPipelineHandler downstream )
    {
      return this._upstream.Pipe( this._downstream.Pipe( downstream ) );
    }
  }
}