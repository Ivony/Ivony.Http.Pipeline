using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// 定义一个 HTTP 请求处理管线
  /// </summary>
  public interface IHttpPipeline
  {

    Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request );

  }
}
