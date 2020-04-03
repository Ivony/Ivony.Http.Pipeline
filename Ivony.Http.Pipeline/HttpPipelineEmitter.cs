using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// HTTP request emitter, emit the HTTP request
  /// </summary>
  public class HttpPipelineEmitter : IHttpPipelineEmitter
  {

    /// <summary>
    /// create <see cref="HttpPipelineEmitter"/> instance
    /// </summary>
    /// <param name="options"></param>
    public HttpPipelineEmitter( HttpPipelineEmitterOptions options )
    {
      Options = options ?? new HttpPipelineEmitterOptions();
    }


    /// <summary>
    /// http emit options
    /// </summary>
    protected HttpPipelineEmitterOptions Options { get; }






    /// <summary>
    /// 实现 ProcessRequest 方法，发送 HTTP 请求并将响应返回
    /// </summary>
    /// <param name="request">要发送的请求</param>
    /// <param name="cancellationToken">取消标识</param>
    /// <returns>响应信息</returns>
    public virtual async ValueTask<HttpResponseMessage> EmitRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {
      if ( Options.ConformHost )
        request.Headers.Host = request.RequestUri.Host;


      if ( Options.EnableRequestChunked == false )
      {
        request.Content = await TryBufferContent( request.Content );
        request.Headers.TransferEncodingChunked = false;
      }


      HttpResponseMessage response;
      try
      {
        response = await Options.HttpClient.SendAsync( request );
      }
      catch ( Exception e )
      {
        if ( e is OperationCanceledException )
          return Timeout( e );

        else
          return BadGateway( e );
      }

      if ( Options.EnableResponseChunked == false )
      {
        response.Content = await TryBufferContent( response.Content );
        response.Headers.TransferEncodingChunked = false;
      }
      else
        response.Headers.TransferEncodingChunked = null;


      response.Headers.ConnectionClose = null;

      return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected virtual HttpResponseMessage BadGateway( Exception exception )
    {

      var response = new HttpResponseMessage( HttpStatusCode.BadGateway );

      if ( Options.EnableDetailedException )
        response.Content = new StringContent( exception.ToString() );

      return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected HttpResponseMessage Timeout( Exception exception )
    {
      var response = new HttpResponseMessage( HttpStatusCode.GatewayTimeout );

      if ( Options.EnableDetailedException )
        response.Content = new StringContent( exception.ToString() );

      return response;
    }

    private async ValueTask<HttpContent> TryBufferContent( HttpContent content )
    {
      if ( content is StreamContent == false )
        return content;


      var data = await content.ReadAsByteArrayAsync();
      var newContent = new ByteArrayContent( data );

      foreach ( var header in content.Headers )
        newContent.Headers.Add( header.Key, header.Value.AsEnumerable() );

      return newContent;
    }
  }
}
