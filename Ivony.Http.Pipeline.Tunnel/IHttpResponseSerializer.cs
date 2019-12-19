using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Tunnel
{
  public interface IHttpResponseSerializer
  {

    ValueTask SerializeAsync( HttpResponseMessage response, Stream stream, CancellationToken cancellationToken = default( CancellationToken ) );

    ValueTask<HttpResponseMessage> DeserializeAsync( Stream stream, CancellationToken cancellationToken = default( CancellationToken ) );

  }
}
