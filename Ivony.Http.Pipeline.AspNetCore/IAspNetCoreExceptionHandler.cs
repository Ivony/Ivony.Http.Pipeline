using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// definition a type to handle exception.
  /// </summary>
  public interface IAspNetCoreExceptionHandler
  {

    /// <summary>
    /// handle exception in http pipeline.
    /// </summary>
    /// <param name="context">HTTP request context</param>
    /// <param name="exception">exception data</param>
    /// <returns>a <see cref="Task"/> object to wait completed</returns>
    Task HandleExceptionAsync( HttpContext context, Exception exception );
  }
}
