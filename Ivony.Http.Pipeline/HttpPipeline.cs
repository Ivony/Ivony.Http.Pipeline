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
    /// <param name="downstream">下游管线</param>
    /// <returns>接入了当前中间件的管线</returns>
    public HttpPipelineHandler Pipe( HttpPipelineHandler downstream )
    {
      NextPipeline = downstream;
      return request => ProcessRequest( request );
    }


    /// <summary>
    /// 通过方法创建一个 IHttpPipeline 对象
    /// </summary>
    /// <param name="pipeline">一个处理管线方法</param>
    /// <returns></returns>
    public static IHttpPipeline Create( Func<HttpPipelineHandler, HttpPipelineHandler> pipeline )
    {
      return new PipelineWrapper( pipeline );
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
      public HttpPipelineHandler Pipe( HttpPipelineHandler downstream )
      {
        return downstream;
      }
    }

    private class PipelineWrapper : IHttpPipeline
    {
      private Func<HttpPipelineHandler, HttpPipelineHandler> _pipeline;

      public PipelineWrapper( Func<HttpPipelineHandler, HttpPipelineHandler> pipeline )
      {
        _pipeline = pipeline;
      }

      public HttpPipelineHandler Pipe( HttpPipelineHandler downstream )
      {
        return _pipeline( downstream );
      }
    }
  }
}
