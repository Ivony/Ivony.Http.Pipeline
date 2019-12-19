using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline.Handlers
{
  /// <summary>
  /// handle request and response HTTP 302 Found.
  /// </summary>
  public class HttpFound : HttpRedirectHandlerBase
  {


    public HttpFound( RewriteRule rule ) : base( rule ) { }


    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {
      return Result( Redirect( request, HttpStatusCode.Found ) );
    }
  }
}
