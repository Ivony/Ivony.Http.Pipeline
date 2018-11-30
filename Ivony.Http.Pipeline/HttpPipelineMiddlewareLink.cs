namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 链接两个管线中间件的链接器
  /// </summary>
  internal sealed class HttpPipelineMiddlewareLink : IHttpPipeline
  {
    private IHttpPipeline middleware;
    private IHttpPipeline nextMiddleware;

    public HttpPipelineMiddlewareLink( IHttpPipeline middleware, IHttpPipeline nextMiddleware )
    {
      this.middleware = middleware;
      this.nextMiddleware = nextMiddleware;
    }

    HttpPipelineHandler IHttpPipeline.Pipe( HttpPipelineHandler pipeline )
    {
      return middleware.Pipe( nextMiddleware.Pipe( pipeline ) );
    }
  }
}