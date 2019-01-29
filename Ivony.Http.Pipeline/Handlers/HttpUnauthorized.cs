using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{
  /// <summary>
  /// handle request and response HTTP 401 Unauthorized.
  /// </summary>
  public class HttpUnauthorized : HttpSpecifiedHandlerBase
  {
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {

      return Result( Response( HttpStatusCode.Unauthorized ) );

    }
  }
}
