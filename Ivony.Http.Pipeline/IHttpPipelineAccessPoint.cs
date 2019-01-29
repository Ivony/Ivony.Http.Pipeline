using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline
{
  /// <summary>
  /// a access point for HTTP pipeline
  /// </summary>
  /// <typeparam name="T">the type to access another HTTP stream, for example RequestDelegate, or HttpMessageHandler.</typeparam>
  public interface IHttpPipelineAccessPoint<T>
  {

    /// <summary>
    /// create a access object to join HTTP pipeline
    /// </summary>
    /// <param name="pipeline">HTTP pipeline</param>
    /// <returns>access object</returns>
    T Combine( IHttpPipelineHandler pipeline );

  }
}
