using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline
{
  public interface IHttpPipelineMiddleware
  {

    IHttpPipeline Pipe( IHttpPipeline pipeline );

  }
}
