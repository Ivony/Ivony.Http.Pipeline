using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public interface IHttpPipelineHandler
  {

    ValueTask<HttpResponseMessage> PrecessRequest( HttpRequestMessage request );

  }
}
