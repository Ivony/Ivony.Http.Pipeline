using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 定义一个处理管线中间件
  /// </summary>
  public interface IHttpPipelineMiddleware
  {

    IHttpPipeline Pipe( IHttpPipeline pipeline );

  }
}
