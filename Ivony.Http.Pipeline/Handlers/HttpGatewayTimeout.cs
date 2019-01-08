using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{
  public class HttpGatewayTimeout : HttpSpecifiedHandlerBase
  {
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {

      return Result( Response( HttpStatusCode.GatewayTimeout ) );

    }
  }
}
