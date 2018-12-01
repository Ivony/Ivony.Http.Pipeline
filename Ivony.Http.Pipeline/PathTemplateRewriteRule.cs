using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Ivony.Http.Pipeline
{
  internal class PathTemplateRewriteRule : IHttpPipelineRouteRule
  {
    private string sourcePathTemplate;
    private string targetPathTemplate;

    public PathTemplateRewriteRule( string sourcePathTemplate, string targetPathTemplate )
    {
      this.sourcePathTemplate = sourcePathTemplate;
      this.targetPathTemplate = targetPathTemplate;
    }

    public IDictionary<string, string> Route( HttpRequestMessage request )
    {
      return GetRouteValues( request );
    }

    protected virtual HttpPipelineHandler PipelineRewrite( HttpPipelineHandler handler )
    {
      return handler;
    }

    protected virtual IDictionary<string, string> GetRouteValues( HttpRequestMessage request )
    {
      throw new NotImplementedException();
    }
  }
}