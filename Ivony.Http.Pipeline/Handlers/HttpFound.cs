using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline.Handlers
{
  public class HttpFound : HttpRedirectHandlerBase
  {


    public HttpFound( RewriteRule rule ) : base( rule ) { }


    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {
      return Result( Redirect( request, HttpStatusCode.Found ) );
    }
  }
}
