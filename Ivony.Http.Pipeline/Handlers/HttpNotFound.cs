using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{

  /// <summary>
  /// handle request and response HTTP 404 Not Found.
  /// </summary>
  public class HttpNotFound : HttpSpecifiedHandlerBase
  {
    /// <summary>
    /// handle request and response HTTP 404
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns>response</returns>
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {
      return Result( Response( HttpStatusCode.NotFound ) );
    }
  }
}
