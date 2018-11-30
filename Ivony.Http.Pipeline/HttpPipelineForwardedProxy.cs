using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public class HttpPipelineForwardedProxy : HttpPipelineMiddleware
  {
    protected override Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request, HttpPipelineHandler nextPipeline )
    {
      var context = request.GetHttpContext();
      request.Headers.Add( "Forwarded", $"for={context.Connection.RemoteIpAddress};proto={context.Request.Protocol};host={context.Request.Host}" );

      return nextPipeline( request );
    }
  }
}