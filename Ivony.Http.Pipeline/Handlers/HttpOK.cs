using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{
  /// <summary>
  /// handle request and response HTTP 200 OK with empty content.
  /// </summary>
  public class HttpOK : HttpSpecifiedHandlerBase
  {
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {
      return Result( Response( HttpStatusCode.OK ) );
    }
  }
}
