﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
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
    /// create a <see cref="IHttpPipeline"/> object by process method.
    /// </summary>
    /// <param name="pipeline">a method to process http pipeline</param>
    /// <returns></returns>
    public static IHttpPipeline Create( Func<IHttpPipelineHandler, IHttpPipelineHandler> pipeline )
    {
      return new PipelineWrapper( pipeline );
    }


    /// <summary>
    /// 派生类重写此方法处理请求
    /// </summary>
    /// <param name="request">request infomation</param>
    /// <param name="downstream">downstream pipeline request handler</param>
    /// <returns>response information</returns>
    protected virtual ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, IHttpPipelineHandler downstream, CancellationToken cancellationToken )
    {
      return downstream.ProcessRequest( request, cancellationToken );
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

      public ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
      {
        return _pipeline.ProcessRequest( request, _downstream, cancellationToken );
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
