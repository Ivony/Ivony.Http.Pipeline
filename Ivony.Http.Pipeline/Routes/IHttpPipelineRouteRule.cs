using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  public interface IHttpPipelineRouteRule
  {

    IDictionary<string, string> Match( RouteRequestData requestData );

  }
}
