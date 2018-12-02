using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  public class RoutePathRewriteRule : IHttpPipelineRouteRule, IHttpPipeline
  {
    public RoutePathTemplate UpstreamTemplate { get; }
    public RoutePathTemplate DownstreamTemplate { get; }

    public RoutePathRewriteRule( RoutePathTemplate upstreamTemplate, RoutePathTemplate downstreamTemplate )
    {
      UpstreamTemplate = upstreamTemplate ?? throw new ArgumentNullException( nameof( upstreamTemplate ) );
      DownstreamTemplate = downstreamTemplate ?? throw new ArgumentNullException( nameof( downstreamTemplate ) );
    }



    public IDictionary<string, string> Route( HttpRequestMessage request )
    {
      return UpstreamTemplate.GetRouteValues( request );
    }

    public HttpPipelineHandler Pipe( HttpPipelineHandler downstream )
    {
      throw new NotImplementedException();
    }


  }
}
