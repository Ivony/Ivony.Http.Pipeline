using System;
using System.Collections.Generic;
using System.Text;
using Ivony.Http.Pipeline;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class AspNetCoreServiceExtensions
  {
    public static IServiceCollection AddHttpPipeline( this IServiceCollection services )
    {
      services.AddSingleton<IHttpPipelineEmitter, HttpPipelineEmitter>();
      services.AddSingleton<IAspNetCoreExceptionHandler, AspNetCoreExceptionHandler>();
      services.AddSingleton<IHttpPipelineAccessPoint<RequestDelegate>, AspNetCoreCombinator>();

      return services;
    }
  }
}
