using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Tunnel
{
  public class HttpTunnelCombinator
  {

    public HttpTunnelCombinator( IHttpTunnel tunnel, IHttpPipelineHandler handler, IHttpRequestSerializer requestSerializer, IHttpResponseSerializer responseSerializer )
    {
      Tunnel = tunnel ?? throw new ArgumentNullException( nameof( tunnel ) );
      Handler = handler;
      RequestSerializer = requestSerializer ?? throw new ArgumentNullException( nameof( requestSerializer ) );
      ResponseSerializer = responseSerializer ?? throw new ArgumentNullException( nameof( responseSerializer ) );
    }

    public IHttpTunnel Tunnel { get; }

    public IHttpPipelineHandler Handler { get; }

    public IHttpRequestSerializer RequestSerializer { get; }

    public IHttpResponseSerializer ResponseSerializer { get; }



    public async Task RunAsync( CancellationToken cancellationToken )
    {
      while ( true )
      {
        cancellationToken.ThrowIfCancellationRequested();

        var connection = await Tunnel.GetConnection( cancellationToken );

        var task = Task.Run( async () =>
        {
          var request = await RequestSerializer.DeserializerAsync( await connection.GetReadStream( cancellationToken ) );
          var response = await Handler.ProcessRequest( request );
          await ResponseSerializer.SerializeAsync( response, await connection.GetWriteStream( cancellationToken ) );

        }, cancellationToken );

      }
    }


  }
}
