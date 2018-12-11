using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline.Tunnel
{
  public interface IHttpRequestSerializer
  {

    void Serialize( HttpRequestMessage request, Stream stream );

    HttpRequestMessage Deserializer( Stream stream );


  }
}
