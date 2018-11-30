using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public interface IHttpPipeline
  {

    Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request );

  }
}
