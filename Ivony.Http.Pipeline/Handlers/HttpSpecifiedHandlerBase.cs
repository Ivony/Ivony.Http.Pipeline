using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline.Handlers
{

  /// <summary>
  /// a base type for specified HTTP handler
  /// </summary>
  public abstract class HttpSpecifiedHandlerBase : IHttpPipelineHandler
  {


    /// <summary>
    /// subtype implement this method to handle http request
    /// </summary>
    /// <param name="request">http request</param>
    /// <returns>http response</returns>
    public abstract ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request );


    /// <summary>
    /// create http response
    /// </summary>
    /// <param name="statusCode">response status code</param>
    /// <returns></returns>
    protected HttpResponseMessage Response( HttpStatusCode statusCode )
    {
      return Response( statusCode, "" );
    }

    /// <summary>
    /// create http response
    /// </summary>
    /// <param name="statusCode">response status code</param>
    /// <param name="content">response content</param>
    /// <returns></returns>
    protected HttpResponseMessage Response( HttpStatusCode statusCode, string content )
    {
      var response = new HttpResponseMessage( statusCode )
      {
        Content = new StringContent( content, UTF8Encoding )
      };

      return response;
    }


    protected Encoding UTF8Encoding { get; } = new UTF8Encoding( false );



    /// <summary>
    /// create ValueTask&lt;HttpResponseMessage&gt; instance as result
    /// </summary>
    /// <param name="response">http response</param>
    /// <returns>wrpped http response instance</returns>
    protected ValueTask<HttpResponseMessage> Result( HttpResponseMessage response )
    {
      return new ValueTask<HttpResponseMessage>( response );
    }

  }
}
