using System;
using System.Collections.Generic;
using System.Text;
using Ivony.Http.Pipeline;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class HttpPipelineServiceExtensions
  {


    public static IServiceCollection AddHttpPipeline( this IServiceCollection services )
    {
      services.AddSingleton<IHttpPipelineEmitter, HttpPipelineEmitter>();
      services.AddSingleton<IHttpPipelineAspNetCoreService, HttpPipelineAspNetCoreService>();

      return services;
    }

  }
}
