using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline
{
  public class HttpPipelineEmitterOptions
  {


    public HttpPipelineEmitterOptions() : this( new HttpClient( CreateDefaultHandler() ) )
    {
    }

    public HttpPipelineEmitterOptions( HttpClient client )
    {
      HttpClient = client ?? throw new ArgumentNullException( nameof( client ) );
    }

    public HttpPipelineEmitterOptions( HttpMessageHandler handler )
    {
      HttpClient = new HttpClient( handler ?? throw new ArgumentNullException( nameof( handler ) ) );
    }

    private static HttpMessageHandler CreateDefaultHandler()
    {
      return new SocketsHttpHandler() { AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.None, UseCookies = false };
    }



    /// <summary>
    /// 用于发出请求的 HttpClient 对象
    /// </summary>
    public HttpClient HttpClient { get; }


    /// <summary>
    /// 禁用请求分块传输，设置为true将强行将请求内容一次性传输
    /// </summary>
    public bool DisableRequestChunked { get; set; } = true;

    /// <summary>
    /// 禁用响应分块传输，设置为true将强行将响应内容一次性读取
    /// </summary>
    public bool DisableResponseChunked { get; set; } = true;

    /// <summary>
    /// 使得 Host 头和 RequestUri 保持一致
    /// </summary>
    public bool ConformHost { get; set; } = true;

  }
}
