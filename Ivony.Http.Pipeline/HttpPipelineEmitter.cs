using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// HTTP request emitter, emit the HTTP request
  /// </summary>
  public class HttpPipelineEmitter : IHttpPipelineEmitter
  {

    public HttpPipelineEmitter() : this( new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.None, UseCookies = false } )
    {
    }


    public HttpPipelineEmitter( HttpMessageHandler handler ) : this( new HttpClient( handler ) )
    {
    }



    public HttpPipelineEmitter( HttpClient client )
    {
      _client = client;
    }

    HttpClient _client;

    /// <summary>
    /// 实现 ProcessRequest 方法，发送 HTTP 请求并将响应返回
    /// </summary>
    /// <param name="request">要发送的请求</param>
    /// <returns>响应信息</returns>
    public virtual async ValueTask<HttpResponseMessage> EmitRequest( HttpRequestMessage request )
    {
      request.Headers.Host = request.RequestUri.Host;

      var response = await _client.SendAsync( request );

      response.Headers.TransferEncodingChunked = null;
      response.Headers.ConnectionClose = null;

      return response;
    }

  }
}
