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
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {

      return Result( Response( HttpStatusCode.Conflict ) );

    }
  }
}
