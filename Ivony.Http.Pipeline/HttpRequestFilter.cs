using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public class HttpRequestFilter : HttpPipeline
  {
    private readonly Func<HttpRequestMessage, HttpRequestMessage> _func;

    public HttpRequestFilter( Func<HttpRequestMessage, HttpRequestMessage> func )
    {
      _func = func ?? throw new ArgumentNullException( nameof( func ) );
    }

    protected override ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, IHttpPipelineHandler downstream, CancellationToken cancellationToken )
    {

      request = _func( request );
      return downstream.ProcessRequest( request, cancellationToken );

    }
  }
}