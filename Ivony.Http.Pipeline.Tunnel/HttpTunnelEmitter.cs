using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Tunnel
{
  public class HttpTunnelEmitter : IHttpPipelineEmitter
  {

    public HttpTunnelEmitter( IHttpTunnel tunnel, IHttpRequestSerializer requestSerializer, IHttpResponseSerializer responseSerializer )
    {
      Tunnel = tunnel ?? throw new ArgumentNullException( nameof( tunnel ) );
      RequestSerializer = requestSerializer ?? throw new ArgumentNullException( nameof( requestSerializer ) );
      ResponseSerializer = responseSerializer ?? throw new ArgumentNullException( nameof( responseSerializer ) );
    }

    public IHttpTunnel Tunnel { get; }

    public IHttpRequestSerializer RequestSerializer { get; }

    public IHttpResponseSerializer ResponseSerializer { get; }

    public async ValueTask<HttpResponseMessage> EmitRequest( HttpRequestMessage request, CancellationToken cancellationToken )
    {

      using ( var connection = await Tunnel.GetConnection() )
      {
        await RequestSerializer.SerializeAsync( request, await connection.GetWriteStream(), cancellationToken );
        return await ResponseSerializer.DeserializeAsync( await connection.GetReadStream(), cancellationToken );
      }
    }
  }
}
