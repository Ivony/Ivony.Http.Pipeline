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
  public abstract class HttpPipeline : IHttpPipeline
  {


    /// <summary>
    /// 下游管线
    /// </summary>
    protected HttpPipelineHandler NextPipeline { get; private set; }

    /// <summary>
    /// 实现 Pipe 方法，将当前中间件接入到管线中
    /// </summary>
    /// <param name="pipeline">下游管线</param>
    /// <returns>接入了当前中间件的管线</returns>
    public HttpPipelineHandler Pipe( HttpPipelineHandler pipeline )
    {
      NextPipeline = pipeline;
      return request => ProcessRequest( request );
    }

    protected virtual Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {
      return NextPipeline( request );
    }

  }
}
