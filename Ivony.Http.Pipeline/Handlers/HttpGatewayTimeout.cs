using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{
  /// <summary>
  /// handle request and response HTTP 504 Gateway Timeout.
  /// </summary>
  public class HttpGatewayTimeout : HttpSpecifiedHandlerBase
  {
    /// <summary>
    /// handle request and response HTTP 504 Gateway Timeout
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns>HTTP 504 response</returns>
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {

      return Result( Response( HttpStatusCode.GatewayTimeout ) );

    }
  }
}
