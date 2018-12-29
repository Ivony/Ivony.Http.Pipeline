using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ivony.Http.Pipeline;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ivony.Http.Pipeline.Routes;

namespace ForwardProxy
{
  public class Program
  {
    public static void Main( string[] args )
    {

      WebHost
        .CreateDefaultBuilder( args )
        .UseStartup<Startup>()
        .Build()
        .Run();

    }

    private class Startup : IStartup
    {
      public void Configure( IApplicationBuilder app )
      {
        app
          .ForwardProxy()
          .UseRouter( configure => configure
            .Rewrite( "//163.com/{path*}", "//jumony.net/{path}" ) )
          .Emit();
      }

      public IServiceProvider ConfigureServices( IServiceCollection services )
      {
        services.AddHttpPipeline();
        return services.BuildServiceProvider();
      }
    }
  }
}
