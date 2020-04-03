using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{

  /// <summary>
  /// handle request and response HTTP 403 Forbidden.
  /// </summary>
  public class HttpForbidden : HttpSpecifiedHandlerBase
  {

    /// <summary>
    /// handle request and response HTTP 403
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns>HTTP 403 response</returns>
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {
      return Result( Response( HttpStatusCode.Forbidden ) );
    }
  }
}
