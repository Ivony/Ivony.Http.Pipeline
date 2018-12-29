The **Http Pipeline** is a HTTP request handler similar to a pipeline. 

It makes it easy for us to develop a variety of HTTP Network Service components based on aspnet core. For example, API gateways, proxies, reverse proxies, response caching, load balancing, and more.

We can see many network services and components as a combination of a series of more basic components. for example:

a LoadBalanacer can be seen as a combination of  Forwarder, Balancer and Emitter.

> **Forwarder** -> **Balancer** -> **Emitter**

**Forwarder** receives and sends requests to the server and forwards them to downstream pipelines.

**Balancer** distribute request to different downstream pipelines in accordance with the rules.

**Emitter** sends a request to the server and returns the response

We just need to write code like this.

```CSharp
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IHostingEnvironment env )
    {

      app.UsePipeline( pipeline => pipeline
        .Forward()
        .UseLoadBalancer(
          pipe => pipe.RewriteHost( "10.0.0.1" ),
          pipe => pipe.RewriteHost( "10.0.0.2" ),
          pipe => pipe.RewriteHost( "10.0.0.3" ),
          pipe => pipe.RewriteHost( "10.0.0.4" ),
          pipe => pipe.RewriteHost( "10.0.0.5" )
        )
        .Emit()
      );

    }
```


So, an API Gateway looks like this:

> **Forwarder** -> **AuthenticationFilter** -> **Router & Rewriter** -> **Balancer** -> **Logger** -> **Emitter**


You can create your own network services with any combination of these basic components.

have fun.


performance test result:

* **40% less average request time** compared to Ocelot
* **more than 25% rps** compared to Ocelot


[![Nuget](http://img.shields.io/nuget/v/Ivony.Http.Pipeline.svg)](https://www.nuget.org/packages/Ivony.Http.Pipeline/)
[![Nuget](http://img.shields.io/nuget/dt/Ivony.Http.Pipeline.svg)](https://www.nuget.org/packages/Ivony.Http.Pipeline/)
	

