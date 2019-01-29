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
    /// create HttpRedirectHandlerBase instance
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
