using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Tunnel
{
  public interface IHttpTunnel
  {

    ValueTask<IHttpTunnelConnection> GetConnection( CancellationToken cancellationToken = default( CancellationToken ) );

  }
}
