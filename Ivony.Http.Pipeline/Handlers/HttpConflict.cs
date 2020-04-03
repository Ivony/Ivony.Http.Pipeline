using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{
  /// <summary>
  /// handle request and response HTTP 409 Conflict.
  /// </summary>
  public class HttpConflict : HttpSpecifiedHandlerBase
  {
    /// <summary>
    /// handle request and response HTTP 409 Conflict
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns>HTTP 409 response</returns>
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {

      return Result( Response( HttpStatusCode.Conflict ) );

    }
  }
}
