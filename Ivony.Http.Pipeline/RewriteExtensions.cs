using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{
  public static class RewriteExtensions
  {
    /// <summary>
    /// 重写请求的 Host 属性
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <param name="host">要重写的主机头</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline RewriteHost( this IHttpPipeline middleware, string host )
    {
      return RewriteHost( middleware, new HostString( host ) );
    }


    /// <summary>
    /// 重写请求的 Host 属性
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <param name="host">要重写的主机头</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline RewriteHost( this IHttpPipeline middleware, HostString host )
    {
      return middleware.Pipe( new HttpRequestFilter( request =>
      {
        var builder = new UriBuilder( request.RequestUri );

        if ( host.HasValue )
        {
          builder.Host = host.Host;
          builder.Port = host.Port ?? GetDefaultPort( builder.Scheme ) ?? builder.Port;

        }


        request.RequestUri = builder.Uri;
        return request;
      } ) );
    }

    private static int? GetDefaultPort( string scheme )
    {
      if ( scheme == Uri.UriSchemeHttp )
        return 80;

      else
        return null;
    }
  }
}
