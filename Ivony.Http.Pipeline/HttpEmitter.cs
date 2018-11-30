﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// HTTP 请求发送，只负责发送 HTTP 请求。
  /// </summary>
  public class HttpEmitter : IHttpPipeline
  {

    HttpClient client = new HttpClient();

    /// <summary>
    /// 实现 ProcessRequest 方法，发送 HTTP 请求并将响应返回
    /// </summary>
    /// <param name="request">要发送的请求</param>
    /// <returns>响应信息</returns>
    public Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {
      return client.SendAsync( request );
    }
  }
}
