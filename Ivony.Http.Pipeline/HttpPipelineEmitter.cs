using System;
using System.Collections.Generic;
using System.Linq;
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

    public HttpPipelineEmitter( HttpPipelineEmitterOptions options )
    {
      Options = options ?? new HttpPipelineEmitterOptions();
    }

    protected HttpPipelineEmitterOptions Options { get; }






    /// <summary>
    /// 实现 ProcessRequest 方法，发送 HTTP 请求并将响应返回
    /// </summary>
    /// <param name="request">要发送的请求</param>
    /// <returns>响应信息</returns>
    public virtual async ValueTask<HttpResponseMessage> EmitRequest( HttpRequestMessage request )
    {
      request.Headers.Host = request.RequestUri.Host;

      if ( Options.DisableRequestChunked )
      {
        if ( request.Content is StreamContent )
        {
          var data = await request.Content.ReadAsByteArrayAsync();
          var content = new ByteArrayContent( data );

          foreach ( var header in request.Content.Headers )
            content.Headers.Add( header.Key, header.Value.AsEnumerable() );

          request.Content = content;
        }

        request.Headers.TransferEncodingChunked = false;
      }

      var response = await Options.HttpClient.SendAsync( request );

      response.Headers.TransferEncodingChunked = null;
      response.Headers.ConnectionClose = null;

      return response;
    }

  }
}
