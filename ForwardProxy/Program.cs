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
using Ivony.Http.Pipeline.Handlers;

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
          .Forward()
          .UseRouter( routes => routes
            .Rewrite( "//nl.miworldtech.com/{path*}", "http://omsapi.miworldtech.com/{path}" )
            .Rewrite( "//nl.label.miworldtech.com/{path*}", "http://omsapilabel.miworldtech.com/{path}" )
            .Rewrite( "//nl.yunexpress.com/order/{path*}", "http://omsapi.miworldtech.com/{path}" )
            .Rewrite( "//nl.yunexpress.com/label/{path*}", "http://omsapilabel.miworldtech.com/{path}" )
            .Otherwise( new HttpNotFound() )
          )
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
