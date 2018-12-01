using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 定义 HTTP 处理管线
  /// </summary>
  /// <param name="request">HTTP 请求信息</param>
  /// <returns>异步获取 HTTP 响应信息的对象</returns>
  public delegate Task<HttpResponseMessage> HttpPipelineHandler( HttpRequestMessage request );


}
