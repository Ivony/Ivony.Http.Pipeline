using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline.Handlers
{
  /// <summary>
  /// handle request and response HTTP 301 Moved Permanently
  /// </summary>
  public class HttpMovedPermanently : HttpRedirectHandlerBase
  {


    /// <summary>
    /// create HttpMovedPermanently instance
    /// </summary>
    /// <param name="rule">rewrite rule for redirect</param>
    public HttpMovedPermanently( RewriteRule rule ) : base( rule ) { }



    /// <summary>
    /// handle request and response HTTP 301 Moved Permanently
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <returns>response</returns>
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {
      return Result( Redirect( request, HttpStatusCode.MovedPermanently ) );
    }
  }
}
