using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{




  public class AspNetCoreForwardCombinator : AspNetCoreCombinator
  {

    public AspNetCoreForwardCombinator( ForwardProxyMode forwardProxyMode, TransmitHeaderBehavior transmitHeaderBehavior )
    {
      ForwardProxyMode = forwardProxyMode;
      TransmitHeaderBehavior = transmitHeaderBehavior;
    }

    public ForwardProxyMode ForwardProxyMode { get; }

    public TransmitHeaderBehavior TransmitHeaderBehavior { get; }



    protected override Task<HttpRequestMessage> CreateRequest( HttpContext context )
    {
      var request = new HttpRequestMessage();
      request.Properties[HttpContextAccessKey] = context;


      request.Method = new HttpMethod( context.Request.Method );
      request.RequestUri = CreateUri( context.Request );

      AddHeaders( context, request );

      return Task.FromResult( request );
    }



    private void AddHeaders( HttpContext context, HttpRequestMessage request )
    {

      switch ( TransmitHeaderBehavior )
      {
        case TransmitHeaderBehavior.All:
          foreach ( var item in context.Request.Headers )
            request.Headers.Add( item.Key, item.Value.AsEnumerable() );
          break;
      }


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
    }

  }
}
