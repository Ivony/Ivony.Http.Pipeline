using System;
using System.Linq;
using System.Net.Http;

namespace Ivony.Http.Pipeline
{
  public static class ExtensionsBuildHttpPipeline
  {






    /// <summary>
    /// 接入一个管线
    /// </summary>
    /// <param name="upstream">上游管线</param>
    /// <param name="downstream">要接入的下游管线</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline JoinPipeline( this IHttpPipeline upstream, IHttpPipeline downstream )
    {
      return new HttpPipelineJointer( upstream, downstream );
    }


    /// <summary>
    /// 接入一个管线
    /// </summary>
    /// <param name="upstream">上游管线</param>
    /// <param name="downstream">要接入的下游管线</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline JoinPipeline( this IHttpPipeline upstream, Func<HttpPipelineHandler, HttpPipelineHandler> downstream )
    {
      return new HttpPipelineJointer( upstream, HttpPipeline.Create( downstream ) );
    }


    /// <summary>
    /// 使用负载均衡器
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="pipelines">下游管线列表</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline UseLoadBalancer( this IHttpPipeline pipeline, params IHttpPipeline[] pipelines )
    {
      return pipeline.JoinPipeline( new HttpPipelineLoadBalancer( pipelines ) );
    }

    /// <summary>
    /// 使用负载均衡器
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="pipelines">下游管线列表</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline UseLoadBalancer( this IHttpPipeline pipeline, params Func<IHttpPipeline, IHttpPipeline>[] pipelinesFactories )
    {
      return pipeline.JoinPipeline( new HttpPipelineLoadBalancer( pipelinesFactories.Select( f => f( pipeline ) ).ToArray() ) );
    }

    /// <summary>
    /// 分发管线
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="distributer">分发器</param>
    /// <returns>请求处理管线</returns>
    public static HttpPipelineHandler Distribute( this IHttpPipeline pipeline, IHttpPipelineDistributer distributer )
    {
      if ( distributer == null )
        throw new ArgumentNullException( nameof( distributer ) );

      return pipeline.Join( request => distributer.Distribute( request )( request ) );
    }

    /// <summary>
    /// 分发管线
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <param name="pipelines">下游管线列表</param>
    /// <returns>请求处理管线</returns>
    public static HttpPipelineHandler Distribute( this IHttpPipeline pipeline, params HttpPipelineHandler[] pipelines )
    {
      if ( pipelines == null )
        throw new ArgumentNullException( nameof( pipelines ) );

      if ( pipelines.Any() == false )
        throw new ArgumentNullException( nameof( pipelines ) );

      return pipeline.Distribute( new HttpPipelineBalanceDistributer( pipelines ) );
    }






    /// <summary>
    /// insert a request logger to pipeline
    /// </summary>
    /// <param name="pipeline">HTTP pipeline</param>
    /// <returns>new HTTP pipeline</returns>
    public static IHttpPipeline UseLogger( this IHttpPipeline pipeline )
    {
      throw new NotImplementedException();
    }



    /// <summary>
    /// join pipeline with a request emitter, create a http pipeline handler.
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <returns>http pipeline handler</returns>
    public static HttpPipelineHandler Emit( this IHttpPipeline pipeline )
    {
      return pipeline.Join( new HttpPipelineEmitter() );
    }



    /// <summary>
    /// join pipeline with a request emitter, create a http pipeline handler.
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="emitter">request emitter</param>
    /// <returns>http pipeline handler</returns>
    public static HttpPipelineHandler Emit( this IHttpPipeline pipeline, IHttpPipelineEmitter emitter )
    {
      return pipeline.Join( emitter.EmitRequest );
    }


    /// <summary>
    /// join pipeline with a request emitter, create a http pipeline handler.
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="handler">http message handler</param>
    /// <returns>http pipeline handler</returns>
    public static HttpPipelineHandler Emit( this IHttpPipeline pipeline, HttpMessageHandler handler )
    {
      return pipeline.Join( new HttpPipelineEmitter( handler ) );
    }


  }
}
