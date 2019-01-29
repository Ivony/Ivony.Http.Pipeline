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
    /// downstream pipeline
    /// </summary>
    protected IHttpPipelineHandler Downstream { get; private set; }

    /// <summary>
    /// join downstream pipeline
    /// </summary>
    /// <param name="downstream">downstream pipeline</param>
    /// <returns></returns>
    public IHttpPipelineHandler Join( IHttpPipelineHandler downstream )
    {
      return new HttpPipelineHandler( this, downstream );
    }


    /// <summary>
    /// 通过方法创建一个 IHttpPipeline 对象
    /// </summary>
    /// <param name="pipeline">一个处理管线方法</param>
    /// <returns></returns>
    public static IHttpPipeline Create( Func<IHttpPipelineHandler, IHttpPipelineHandler> pipeline )
    {
      return new PipelineWrapper( pipeline );
    }


    /// <summary>
    /// 派生类重写此方法处理请求
    /// </summary>
    /// <param name="request">请求信息</param>
    /// <param name="downstream">downstream pipeline request handler</param>
    /// <returns>响应信息</returns>
    protected virtual ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, IHttpPipelineHandler downstream )
    {
      return downstream.ProcessRequest( request );
    }



    /// <summary>
    /// 获取一个空白管道，该管道不在管线上增加任何操作
    /// </summary>
    public static IHttpPipeline Blank { get; } = new BlankPipeline();



    private class HttpPipelineHandler : IHttpPipelineHandler
    {
      private readonly HttpPipeline _pipeline;
      private readonly IHttpPipelineHandler _downstream;

      public HttpPipelineHandler( HttpPipeline pipeline, IHttpPipelineHandler downstream )
      {
        _pipeline = pipeline;
        _downstream = downstream;
      }

      public ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
      {
        return _pipeline.ProcessRequest( request, _downstream );
      }
    }

    private class BlankPipeline : IHttpPipeline
    {
      public IHttpPipelineHandler Join( IHttpPipelineHandler downstream )
      {
        return downstream;
      }

      public override string ToString()
      {
        return "::";
      }
    }

    private class PipelineWrapper : IHttpPipeline
    {
      private readonly Func<IHttpPipelineHandler, IHttpPipelineHandler> _pipeline;

      public PipelineWrapper( Func<IHttpPipelineHandler, IHttpPipelineHandler> pipeline )
      {
        _pipeline = pipeline;
      }

      public IHttpPipelineHandler Join( IHttpPipelineHandler downstream )
      {
        return _pipeline( downstream );
      }
    }
  }
}
