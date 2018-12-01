using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 辅助实现 IHttpPipeline 接口
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


    /// <summary>
    /// 派生类重写此方法处理请求
    /// </summary>
    /// <param name="request">请求信息</param>
    /// <returns>响应信息</returns>
    protected virtual Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {
      return NextPipeline( request );
    }



    /// <summary>
    /// 获取一个空白管道，该管道不在管线上增加任何操作
    /// </summary>
    public static IHttpPipeline Blank { get; } = new BlankPipeline();


    private class BlankPipeline : IHttpPipeline
    {
      public HttpPipelineHandler Pipe( HttpPipelineHandler pipeline )
      {
        return pipeline;
      }
    }
  }
}
