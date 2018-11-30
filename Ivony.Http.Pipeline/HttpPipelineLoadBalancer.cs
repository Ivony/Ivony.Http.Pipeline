using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public class HttpPipelineLoadBalancer : IHttpPipeline
  {
    private readonly IHttpPipeline[] _pipelines;

    public HttpPipelineLoadBalancer( params IHttpPipeline[] pipelines )
    {
      _pipelines = pipelines ?? throw new ArgumentNullException( nameof( pipelines ) );
    }




    public HttpPipelineHandler Pipe( HttpPipelineHandler handler )
    {
      var dispatcher = new HttpPipelineDispatcher( _pipelines.Select( item => item.Pipe( handler ) ).ToArray() );

      return request => dispatcher.Dispatch( request )( request );

    }
  }
}
