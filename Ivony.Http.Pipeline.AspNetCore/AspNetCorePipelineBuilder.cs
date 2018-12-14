using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{
  public class AspNetCorePipelineBuilder : IDummyHttpPipeline
  {



    public IHttpPipeline ForwardProxy( ForwardProxyMode mode )
    {
      return new AspNetCoreForwardProxy( mode ).AsPipeline( result => Application = result );
    }

    public IHttpPipeline AsPipeline()
    {
      return new AspNetCoreCombinator().AsPipeline( result => Application = result );
    }

    public IHttpPipelineHandler Join( IHttpPipelineHandler downstream )
    {
      return AsPipeline().Join( downstream );
    }

    internal RequestDelegate Application { get; private set; }

  }
}
