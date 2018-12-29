using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline.Handlers
{
  /// <summary>
  /// handle request and response HTTP 301 Moved Permanently
  /// </summary>
  public class HttpMovedPermanently : IHttpPipelineHandler
  {


    public HttpMovedPermanently( RewriteRule rule )
    {
      Rule = rule;
    }

    public RewriteRule Rule { get; }

    /// <summary>
    /// handle request and response HTTP 301 Moved Permanently
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <returns>response</returns>
    public ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {
      return new ValueTask<HttpResponseMessage>( CreateResponse( Rule.Rewrite( request ).RequestUri ) );
    }

    protected HttpResponseMessage CreateResponse( Uri redirectUrl )
    {
      var response = new HttpResponseMessage( HttpStatusCode.MovedPermanently );
      response.Headers.Location = redirectUrl;

      return response;
    }
  }
}
