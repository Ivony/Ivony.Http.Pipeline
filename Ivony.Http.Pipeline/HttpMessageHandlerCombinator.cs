using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// implement a combinator to combine HTTP pipeline and HttpClient
  /// </summary>
  public class HttpMessageHandlerCombinator : IHttpPipelineAccessPoint<HttpMessageHandler>
  {
    public HttpMessageHandler Combine( IHttpPipelineHandler pipeline )
    {
      return new HttpHandler( pipeline );
    }


    private class HttpHandler : HttpMessageHandler
    {
      public HttpHandler( IHttpPipelineHandler handler )
      {
        Handler = handler;
      }

      public IHttpPipelineHandler Handler { get; }

      protected override Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
      {
        return Handler.ProcessRequest( request ).AsTask();
      }
    }
  }
}
