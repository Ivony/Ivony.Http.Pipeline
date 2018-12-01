using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline
{
  public class HttpPipelineRouteBuilder
  {

    public void AddRule( Predicate<HttpRequestMessage> condition, Func<IHttpPipeline, IHttpPipeline> pipeline )
    {

    }

    public void AddRule( Predicate<HttpRequestMessage> condition, IHttpPipeline pipeline )
    {

    }


  }
}
