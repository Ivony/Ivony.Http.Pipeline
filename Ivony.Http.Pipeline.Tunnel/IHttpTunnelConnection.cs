using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Tunnel
{
  public interface IHttpTunnelConnection : IDisposable
  {
    ValueTask<Stream> GetWriteStream( CancellationToken cancellationToken = default( CancellationToken ) );

    ValueTask<Stream> GetReadStream( CancellationToken cancellationToken = default( CancellationToken ) );
  }
}
