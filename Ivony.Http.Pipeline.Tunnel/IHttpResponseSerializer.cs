using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline.Tunnel
{
  public interface IHttpResponseSerializer
  {

    void Serialize( HttpResponseMessage response, Stream stream );

    HttpResponseMessage Deserializer( Stream stream );

  }
}
