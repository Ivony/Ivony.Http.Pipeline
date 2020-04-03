using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Ivony.Http.Pipeline.Routes;

namespace Ivony.Http.Pipeline.Handlers
{
  /// <summary>
  /// a base type for HTTP Redirect response handler.
  /// </summary>
  public abstract class HttpRedirectHandlerBase : HttpSpecifiedHandlerBase
  {

    /// <summary>
    /// create <see cref="HttpRedirectHandlerBase"/> instance
    /// </summary>
    /// <param name="rule">rewrite rule for redirect</param>
    protected HttpRedirectHandlerBase( RewriteRule rule )
    {
      RewriteRule = rule;
    }


    /// <summary>
    /// rewrite rule for redirect
    /// </summary>
    public RewriteRule RewriteRule { get; }


    /// <summary>
    /// create HTTP redirect response
    /// </summary>
    /// <param name="request">HTTP request message</param>
    /// <param name="statusCode">HTTP Status Code, only accept 301 and 302</param>
    /// <returns>HTTP redirect response</returns>
    protected HttpResponseMessage Redirect( HttpRequestMessage request, HttpStatusCode statusCode )
    {
      var _code = (int) statusCode;
      if ( _code != 301 && _code != 302 )
        throw new ArgumentException( "status code is not redirect status code", "statusCode" );

      var response = Response( statusCode );
      response.Headers.Location = RedirectUrl( request );
      return response;
    }

    private Uri RedirectUrl( HttpRequestMessage request )
    {
      return RewriteRule.Rewrite( request ).RequestUri;
    }
  }
}
