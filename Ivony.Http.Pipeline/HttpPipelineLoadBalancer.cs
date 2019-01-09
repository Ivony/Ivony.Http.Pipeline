using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// http load balancer implements
  /// </summary>
  public class HttpPipelineLoadBalancer : IHttpPipeline
  {
    private readonly IHttpPipeline[] _pipelines;

    public HttpPipelineLoadBalancer( params IHttpPipeline[] pipelines )
    {
      _pipelines = pipelines ?? throw new ArgumentNullException( nameof( pipelines ) );
    }



    /// <summary>
    /// ijoin a downstream pipeline
    /// </summary>
    /// <param name="downstream">downstream pipeline</param>
    /// <returns>http pipeline handler</returns>
    public IHttpPipelineHandler Join( IHttpPipelineHandler downstream )
    {
      var distributer = new HttpPipelineBalanceDistributer( _pipelines.Select( item => item.Join( downstream ) ).ToArray() );
      return distributer.AsHandler();

    }
  }
}
