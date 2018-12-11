using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ivony.Http.Pipeline.Tunnel
{
  public interface IHttpTunnel
  {

    Stream WriteStream { get; }

    Stream ReadStream { get; }

  }
}
