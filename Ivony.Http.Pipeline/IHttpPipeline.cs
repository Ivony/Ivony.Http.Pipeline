using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 定义一个 HTTP 处理管道，管道是 HTTP 处理管线的中间件
  /// </summary>
  public interface IHttpPipeline
  {

    /// <summary>
    /// 链接管线
    /// </summary>
    /// <param name="downstream">下游处理管线</param>
    /// <returns>链接后的 HTTP 处理管线</returns>
    IHttpPipelineHandler Join( IHttpPipelineHandler downstream );

  }
}
