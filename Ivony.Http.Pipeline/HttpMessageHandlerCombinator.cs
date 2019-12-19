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

    /// <summary>
    /// create a HttpMessageHandler instance to access HTTP pipeline.
    /// </summary>
    /// <param name="pipeline"></param>
    /// <returns></returns>
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
        return Handler.ProcessRequest( request, cancellationToken ).AsTask();
      }
    }
  }
}
