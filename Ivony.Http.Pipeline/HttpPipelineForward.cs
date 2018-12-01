using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{
  public class HttpPipelineForward : HttpPipeline
  {

    protected override Task<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
    {
      var context = request.GetHttpContext();

      request.Method = new HttpMethod( context.Request.Method );
      request.RequestUri = CreateUri( context.Request );

      return base.ProcessRequest( request );
    }

    private Uri CreateUri( HttpRequest request )
    {
      var builder = new UriBuilder();
      builder.Scheme = request.Scheme;
      if ( request.Host.HasValue )
      {
        builder.Host = request.Host.Host;
        if ( request.Host.Port.HasValue )
          builder.Port = request.Host.Port.Value;
      }

      builder.Path = request.Path;
      builder.Query = request.QueryString.Value;

      return builder.Uri;
    }
  }
}
