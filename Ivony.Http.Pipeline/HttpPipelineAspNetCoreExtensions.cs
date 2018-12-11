using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Ivony.Http.Pipeline;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class HttpPipelineAspNetCoreExtensions
  {


    public static IServiceCollection AddHttpPipeline( this IServiceCollection services )
    {
      services.AddSingleton<IHttpPipelineEmitter, HttpPipelineEmitter>();
      services.AddSingleton<IHttpPipelineAspNetCoreCombinator, HttpPipelineAspNetCoreCombinator>();

      return services;
    }

  }
}
