using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{
  /// <summary>
  /// handle request and response HTTP 502 Bad Gateway.
  /// </summary>
  public class HttpBadGateway : HttpSpecifiedHandlerBase
  {
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {

      return Result( Response( HttpStatusCode.BadGateway ) );

    }
  }
}
