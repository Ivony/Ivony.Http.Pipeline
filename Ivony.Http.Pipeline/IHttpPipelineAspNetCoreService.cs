using System;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{
  public interface IHttpPipelineAspNetCoreService
  {
    Func<RequestDelegate, RequestDelegate> CreateMiddleware( HttpPipelineHandler pipeline );
  }
}