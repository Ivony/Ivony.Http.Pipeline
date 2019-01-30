using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Tunnel
{
  public class NetworkConnection : IHttpTunnelConnection
  {

    private readonly NetworkStream _stream;

    public NetworkConnection( NetworkStream stream )
    {
      this._stream = stream;
    }


    public void Dispose()
    {
      _stream.Dispose();
    }

    public ValueTask<Stream> GetReadStream( CancellationToken cancellationToken )
    {
      return new ValueTask<Stream>( _stream );
    }

    public ValueTask<Stream> GetWriteStream( CancellationToken cancellationToken )
    {
      return new ValueTask<Stream>( _stream );
    }
  }
}
