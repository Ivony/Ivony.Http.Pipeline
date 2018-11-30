using System;
using System.Net.Http;
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

    protected override Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {

      request = _func( request );
      return base.ProcessRequest( request );

    }
  }
}