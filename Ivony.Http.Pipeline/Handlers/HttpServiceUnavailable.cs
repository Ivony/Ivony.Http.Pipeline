using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{

  /// <summary>
  /// handle request and response HTTP 503 Service Unavailable
  /// </summary>
  public class HttpServiceUnavailable : HttpSpecifiedHandlerBase
  {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {
      return Result( Response( HttpStatusCode.ServiceUnavailable ) );
    }
  }
}
