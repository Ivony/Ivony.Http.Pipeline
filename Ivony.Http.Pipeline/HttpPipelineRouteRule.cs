using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public abstract class HttpPipelineRouteRule : IHttpPipeline
  {
    public abstract bool CheckCondition( HttpRequestMessage request );

    public abstract Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request );


  }
}
