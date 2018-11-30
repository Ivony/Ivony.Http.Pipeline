using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline
{
  public class HttpPipelineDispatcherBuilder
  {

    public HttpPipelineDispatcher Build()
    {
      return new HttpPipelineDispatcher();
    }

  }
}
