using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public class AspNetCoreExceptionHandler : IAspNetCoreExceptionHandler
  {
    private readonly ILogger<AspNetCoreExceptionHandler> _logger;

    public AspNetCoreExceptionHandler( ILogger<AspNetCoreExceptionHandler> logger )
    {
      _logger = logger;
    }

    public Task HandleExceptionAsync( HttpContext context, Exception exception )
    {

      _logger.LogInformation( $"unhandled exception in process pipeline.\n{exception}" );
      context.Response.StatusCode = 502;
      return Task.CompletedTask;

    }
  }
}
