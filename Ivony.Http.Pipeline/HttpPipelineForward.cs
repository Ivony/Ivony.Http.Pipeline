using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// contract proxy headers build mode
  /// </summary>
  public enum ForwardProxyMode
  {
    /// <summary>just forward the request, not as a proxy</summary>
    None,

    /// <summary>build legacy proxy headers, like X-Forwarded-For, X-Forwarded-Host and X-Forwarded-Proto</summary>
    Legacy,

    /// <summary>build proxy headers using RFC7329</summary>
    RFC7239
  }



  /// <summary>
  /// contract transmit headers behavior
  /// </summary>
  public enum TransmitHeaderBehavior
  {
    /// <summary>dose not transmit any header.</summary>
    Nothing,

    /// <summary>transmit all headers.</summary>
    All
  }


  public class HttpPipelineForward : HttpPipeline
  {



    public HttpPipelineForward( ForwardProxyMode forwardProxyMode, TransmitHeaderBehavior transmitHeaderBehavior )
    {
      ForwardProxyMode = forwardProxyMode;
      TransmitHeaderBehavior = transmitHeaderBehavior;
    }

    public ForwardProxyMode ForwardProxyMode { get; }

    public TransmitHeaderBehavior TransmitHeaderBehavior { get; }



    protected override Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {
      var context = request.GetHttpContext();

      request.Method = new HttpMethod( context.Request.Method );
      request.RequestUri = CreateUri( context.Request );

      AddForwardHeaders( request );


      return base.ProcessRequest( request );
    }

    private void AddForwardHeaders( HttpRequestMessage request )
    {
      switch ( ForwardProxyMode )
      {
        case ForwardProxyMode.None:
          break;

        case ForwardProxyMode.Legacy:
          var context = request.GetHttpContext();

          request.Headers.Add( "X-Forwarded-For", context.Connection.LocalIpAddress.ToString() );
          request.Headers.Add( "X-Forwarded-Host", context.Request.Host.Value );
          request.Headers.Add( "X-Forwarded-Proto", context.Request.Protocol );

          break;

        case ForwardProxyMode.RFC7239:
          throw new NotSupportedException();
      }
    }

    private Uri CreateUri( HttpRequest request )
    {
      var builder = new UriBuilder();
      builder.Scheme = request.Scheme;
      if ( request.Host.HasValue )
      {
        builder.Host = request.Host.Host;
        if ( request.Host.Port.HasValue )
          builder.Port = request.Host.Port.Value;
      }

      builder.Path = request.Path;
      builder.Query = request.QueryString.Value;

      return builder.Uri;
    }
  }
}
