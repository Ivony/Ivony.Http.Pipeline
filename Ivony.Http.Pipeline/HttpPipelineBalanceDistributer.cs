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

    public IHttpPipelineHandler[] Pipelines { get; }

    /// <summary>
    /// choose a downstream pipeline to distribute.
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <returns>downstream pipeline to handle this request</returns>
    public virtual IHttpPipelineHandler Distribute( HttpRequestMessage request )
    {
      Interlocked.Increment( ref counter );
      var index = counter = counter % Pipelines.Length;
      return Pipelines[index];
    }



    /// <summary>
    /// create HttpPipelineDispatcher instance
    /// </summary>
    /// <param name="pipelines">下游管线</param>
    public HttpPipelineBalanceDistributer( params IHttpPipelineHandler[] pipelines )
    {
      Pipelines = pipelines ?? throw new ArgumentNullException( nameof( pipelines ) );
    }

  }
}
