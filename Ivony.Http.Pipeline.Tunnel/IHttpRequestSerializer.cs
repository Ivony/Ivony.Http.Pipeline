using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Tunnel
{
  public interface IHttpRequestSerializer
  {

    ValueTask SerializeAsync( HttpRequestMessage request, Stream stream );

    ValueTask<HttpRequestMessage> DeserializerAsync( Stream stream );


  }
}
