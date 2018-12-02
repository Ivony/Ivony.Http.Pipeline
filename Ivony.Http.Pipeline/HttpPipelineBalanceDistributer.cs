using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// HTTP 管线分发器，将 HTTP 请求分发给多个下游管线。
  /// </summary>
  public class HttpPipelineBalanceDistributer : IHttpPipelineDistributer
  {

    private volatile int counter;

    public HttpPipelineHandler[] Pipelines { get; }

    /// <summary>
    /// 获取下游管线
    /// </summary>
    /// <param name="request">HTTP 请求数据</param>
    /// <returns>要处理该请求的下游管线</returns>
    public virtual HttpPipelineHandler Distribute( HttpRequestMessage request )
    {
      Interlocked.Increment( ref counter );
      var index = counter = counter % Pipelines.Length;
      return Pipelines[index];
    }



    /// <summary>
    /// 创建 HttpPipelineDispatcher 对象
    /// </summary>
    /// <param name="pipelines">下游管线</param>
    public HttpPipelineBalanceDistributer( params HttpPipelineHandler[] pipelines )
    {
      Pipelines = pipelines ?? throw new ArgumentNullException( nameof( pipelines ) );
    }

  }
}
