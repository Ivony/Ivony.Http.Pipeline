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
  /// 负载均衡器
  /// </summary>
  public class HttpPipelineLoadBalancer : IHttpPipeline
  {
    private readonly IHttpPipeline[] _pipelines;

    public HttpPipelineLoadBalancer( params IHttpPipeline[] pipelines )
    {
      _pipelines = pipelines ?? throw new ArgumentNullException( nameof( pipelines ) );
    }




    public HttpPipelineHandler Pipe( HttpPipelineHandler downstream )
    {
      var distributer = new HttpPipelineBalanceDistributer( _pipelines.Select( item => item.Pipe( downstream ) ).ToArray() );
      return distributer.AsHandler();

    }
  }
}
