namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 链接两个管线中间件的链接器
  /// </summary>
  internal sealed class HttpPipelineMiddlewareLink : IHttpPipeline
  {
    private readonly IHttpPipeline _upstream;
    private readonly IHttpPipeline _downstream;

    public HttpPipelineMiddlewareLink( IHttpPipeline upstream, IHttpPipeline downstream )
    {
      _upstream = upstream;
      _downstream = downstream;
    }

    HttpPipelineHandler IHttpPipeline.Join( HttpPipelineHandler handler )
    {
      return _upstream.Join( _downstream.Join( handler ) );
    }
  }
}