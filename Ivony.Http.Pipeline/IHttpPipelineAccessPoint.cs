using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline
{
  public interface IHttpPipelineAccessPoint<T>
  {

    T Combine( HttpPipelineHandler pipeline );

  }
}
