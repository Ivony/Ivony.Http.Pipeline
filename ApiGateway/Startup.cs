using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Ivony.Http.Pipeline;

namespace ApiGateway
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices( IServiceCollection services )
    {

      services.AddHttpPipeline();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IHostingEnvironment env )
    {
      if ( env.IsDevelopment() )
      {
        app.UseDeveloperExceptionPage();
      }


      app
        .ForwardProxy()
        .Rewrite( "/{path*}", "//www.163.com/{path}" )
        .Emit();
      /*
              .UseRouter( route => route
                .Match( "/Logs/{path*}" )
                .Match( "/Log/{path*}" )
                .Rewrite( "http://10.168.95.71:5000/{path}" )
                .Match( "/MQ/{path*}" )
                .Rewrite( "http://10.168.95.72:5000/{path}" )
              )
              .Emit();
      */
      app.Run( async ( context ) =>
       {
         await context.Response.WriteAsync( "Hello World!" );
       } );
    }
  }
}
