using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline.Tunnel
{
  public class HttpTunnelCombinator : IHttpPipeline
  {

    public HttpTunnelCombinator( IHttpTunnel tunnel, IHttpRequestSerializer requestSerializer, IHttpResponseSerializer responseSerializer )
    {
      Tunnel = tunnel ?? throw new ArgumentNullException( nameof( tunnel ) );
      RequestSerializer = requestSerializer ?? throw new ArgumentNullException( nameof( requestSerializer ) );
      ResponseSerializer = responseSerializer ?? throw new ArgumentNullException( nameof( responseSerializer ) );
    }

    public IHttpTunnel Tunnel { get; }

    public IHttpRequestSerializer RequestSerializer { get; }

    public IHttpResponseSerializer ResponseSerializer { get; }


    public IHttpPipelineHandler Join( IHttpPipelineHandler downstream )
    {
      throw new NotImplementedException();
    }
  }
}
