using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 辅助实现 IHttpPipelineMiddleware 接口
  /// </summary>
  public abstract class HttpPipelineMiddleware : IHttpPipelineMiddleware
  {
    /// <summary>
    /// 实现 Pipe 方法，将当前中间件接入到管线中
    /// </summary>
    /// <param name="pipeline">下游管线</param>
    /// <returns>接入了当前中间件的管线</returns>
    public IHttpPipeline Pipe( IHttpPipeline pipeline )
    {
      return new Pipeline( request => ProcessRequest( request, pipeline ) );
    }

    protected abstract Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, IHttpPipeline nextPipeline );


    private class Pipeline : IHttpPipeline
    {

      public Pipeline( Func<HttpRequestMessage, Task<HttpResponseMessage>> func )
      {
        Func = func;
      }

      public Func<HttpRequestMessage, Task<HttpResponseMessage>> Func { get; }

      public Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
      {
        return Func( request );
      }
    }
  }
}
