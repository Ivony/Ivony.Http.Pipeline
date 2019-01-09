using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// Implement a balanced pipeline distributor that distributes HTTP requests as balanced as possible to multiple downstream pipelines.
  /// </summary>
  public class HttpPipelineBalanceDistributer : IHttpPipelineDistributer
  {

    private volatile int counter;


    /// <summary>
    /// downstream pipelines to be distributed 
    /// </summary>
    public IReadOnlyList<IHttpPipelineHandler> Downstreams { get; }

    /// <summary>
    /// choose a downstream pipeline to distribute.
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <returns>downstream pipeline to handle this request</returns>
    public virtual IHttpPipelineHandler Distribute( HttpRequestMessage request )
    {
      Interlocked.Increment( ref counter );
      var index = counter = counter % Downstreams.Count;
      return Downstreams[index];
    }



    /// <summary>
    /// create HttpPipelineDispatcher instance
    /// </summary>
    /// <param name="downstreams">downstream pipelines to be distribute</param>
    public HttpPipelineBalanceDistributer( params IHttpPipelineHandler[] downstreams ) : this( (IReadOnlyList<IHttpPipelineHandler>) downstreams )
    {

    }

    /// <summary>
    /// create HttpPipelineDispatcher instance
    /// </summary>
    /// <param name="downstreams">downstream pipelines to be distribute</param>
    public HttpPipelineBalanceDistributer( IReadOnlyList<IHttpPipelineHandler> downstreams )
    {
      Downstreams = downstreams ?? throw new ArgumentNullException( nameof( downstreams ) );
    }

  }
}
