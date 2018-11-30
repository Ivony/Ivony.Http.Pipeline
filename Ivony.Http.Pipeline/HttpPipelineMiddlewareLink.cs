namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 链接两个管线中间件的链接器
  /// </summary>
  internal sealed class HttpPipelineMiddlewareLink : IHttpPipelineMiddleware
  {
    private IHttpPipelineMiddleware middleware;
    private IHttpPipelineMiddleware nextMiddleware;

    public HttpPipelineMiddlewareLink( IHttpPipelineMiddleware middleware, IHttpPipelineMiddleware nextMiddleware )
    {
      this.middleware = middleware;
      this.nextMiddleware = nextMiddleware;
    }

    IHttpPipeline IHttpPipelineMiddleware.Pipe( IHttpPipeline pipeline )
    {
      return middleware.Pipe( nextMiddleware.Pipe( pipeline ) );
    }
  }
}