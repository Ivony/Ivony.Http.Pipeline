using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// contract proxy headers build mode
  /// </summary>
  public enum ForwardProxyMode
  {
    /// <summary>just forward the request, not as a proxy</summary>
    None,

    /// <summary>build legacy proxy headers, like X-Forwarded-For, X-Forwarded-Host and X-Forwarded-Proto</summary>
    Legacy,

    /// <summary>build proxy headers using RFC7329</summary>
    RFC7239
  }



  public class AspNetCoreForwardProxy : AspNetCoreCombinator
  {

    public AspNetCoreForwardProxy( ForwardProxyMode forwardProxyMode )
    {
      ForwardProxyMode = forwardProxyMode;
    }

    public ForwardProxyMode ForwardProxyMode { get; }



    protected override async Task<HttpRequestMessage> CreateRequest( HttpContext context )
    {
      var request = await base.CreateRequest( context );

      switch ( ForwardProxyMode )
      {
        case ForwardProxyMode.None:
          break;

        case ForwardProxyMode.Legacy:
          request.Headers.Add( "X-Forwarded-For", context.Connection.LocalIpAddress.ToString() );
          request.Headers.Add( "X-Forwarded-Host", context.Request.Host.Value );
          request.Headers.Add( "X-Forwarded-Proto", context.Request.Protocol );

          break;

        case ForwardProxyMode.RFC7239:
          throw new NotSupportedException();
      }


      return request;
    }
  }
}
