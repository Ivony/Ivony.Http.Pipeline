using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 实现一个基于条件分发的 HTTP 管线分发器
  /// </summary>
  public class HttpPipelineConditionDistributer : IHttpPipelineDistributer
  {

    /// <summary>
    /// 创建 HttpPipelineConditionDispatcher 对象
    /// </summary>
    /// <param name="rules">分发规则</param>
    /// <param name="defaultHandler">默认处理管线</param>
    public HttpPipelineConditionDistributer( Func<HttpRequestMessage, IHttpPipelineHandler>[] rules, IHttpPipelineHandler defaultHandler )
    {
      Rules = rules ?? throw new ArgumentNullException( nameof( rules ) );
      DefaultHandler = defaultHandler ?? throw new ArgumentNullException( nameof( defaultHandler ) );
    }


    /// <summary>
    /// 分发规则
    /// </summary>
    protected virtual Func<HttpRequestMessage, IHttpPipelineHandler>[] Rules { get; }

    /// <summary>
    /// 默认处理管线（所有条件不满足时需要使用的管线）
    /// </summary>
    protected virtual IHttpPipelineHandler DefaultHandler { get; }


    /// <summary>
    /// 分发 HTTP 请求
    /// </summary>
    /// <param name="request">HTTP 请求信息</param>
    /// <returns>处理管线</returns>
    public IHttpPipelineHandler Distribute( HttpRequestMessage request )
    {
      foreach ( var ruleEntry in Rules )
      {
        var handler = ruleEntry( request );
        if ( handler != null )
          return handler;
      }

      return DefaultHandler;

    }
  }
}
