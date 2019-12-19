using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Tunnel
{


  /// <summary>
  /// define a interface to create a tunneling connection
  /// </summary>
  public interface IHttpTunnel
  {

    /// <summary>
    /// get a tunneling connection to transfer data
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<IHttpTunnelConnection> GetConnection( CancellationToken cancellationToken = default( CancellationToken ) );

  }
}
