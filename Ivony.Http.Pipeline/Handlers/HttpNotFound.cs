using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{

  /// <summary>
  /// handle request and response HTTP 404 Not Found.
  /// </summary>
  public class HttpNotFound : IHttpPipelineHandler
  {
    /// <summary>
    /// handle request and response HTTP 404
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <returns>response</returns>
    public ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {
      return new ValueTask<HttpResponseMessage>( new HttpResponseMessage( System.Net.HttpStatusCode.NotFound )
      {
        Content = new ByteArrayContent( new byte[0] )
      } );
    }
  }
}
