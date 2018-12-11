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
  /// load balancer
  /// </summary>
  public class HttpPipelineLoadBalancer : IHttpPipeline
  {
    private readonly IHttpPipeline[] _pipelines;

    public HttpPipelineLoadBalancer( params IHttpPipeline[] pipelines )
    {
      _pipelines = pipelines ?? throw new ArgumentNullException( nameof( pipelines ) );
    }




    public HttpPipelineHandler Join( HttpPipelineHandler downstream )
    {
      var distributer = new HttpPipelineBalanceDistributer( _pipelines.Select( item => item.Join( downstream ) ).ToArray() );
      return distributer.AsHandler();

    }
  }
}
