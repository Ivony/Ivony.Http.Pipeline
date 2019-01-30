using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Tunnel
{
  public interface IHttpResponseSerializer
  {

    ValueTask Serialize( HttpResponseMessage response, Stream stream );

    ValueTask<HttpResponseMessage> DeserializeAsync( Stream stream );

  }
}
