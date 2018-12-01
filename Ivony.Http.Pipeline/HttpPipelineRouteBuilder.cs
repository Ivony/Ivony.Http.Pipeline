using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline
{
  /// <summary>
  /// 路由表构建帮助程序
  /// </summary>
  public class HttpPipelineRouteBuilder
  {

    /// <summary>
    /// 添加路由规则
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="pipelineFactory"></param>
    public void AddRule( Predicate<HttpRequestMessage> condition, Func<IHttpPipeline, IHttpPipeline> pipelineFactory )
    {
      AddRule( condition, pipelineFactory( HttpPipeline.Blank ) );
    }

    /// <summary>
    /// 添加路由规则
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="pipeline"></param>
    public void AddRule( Predicate<HttpRequestMessage> condition, IHttpPipeline pipeline )
    {

    }


  }
}
