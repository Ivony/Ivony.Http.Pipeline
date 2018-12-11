using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{
  public static class HttpContextExtensions
  {
    /// <summary>
    /// get the HttpContext object
    /// </summary>
    /// <param name="request">request message</param>
    /// <returns>HttpContext object</returns>
    public static HttpContext GetHttpContext( this HttpRequestMessage request )
    {
      if ( request.Properties.TryGetValue( AspNetCoreCombinator.HttpContextAccessKey, out var value ) )
        return (HttpContext) value;

      else
        return null;
    }


  }
}
