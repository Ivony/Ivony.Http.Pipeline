using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// 提供 HTTP 请求管线的帮助方法
  /// </summary>
  public static class ExtensionsHttpPipeline
  {


    /// <summary>
    /// 让管线分发器处理指定的请求
    /// </summary>
    /// <param name="distributer">管线分发器</param>
    /// <param name="request">要处理的请求</param>
    /// <returns>处理结果</returns>
    public static ValueTask<HttpResponseMessage> Handle( this IHttpPipelineDistributer distributer, HttpRequestMessage request )
    {
      return new DistributerHandler( distributer ).ProcessRequest( request );
    }



    /// <summary>
    /// returns the request distributer as a pipeline handler.
    /// </summary>
    /// <param name="distributer">HTTP request distributer</param>
    /// <returns>a pipeline handler</returns>
    public static IHttpPipelineHandler AsHandler( this IHttpPipelineDistributer distributer )
    {
      return new DistributerHandler( distributer );
    }

    private class DistributerHandler : IHttpPipelineHandler
    {
      private readonly IHttpPipelineDistributer _distributer;

      public DistributerHandler( IHttpPipelineDistributer distributer )
      {
        _distributer = distributer;
      }

      public ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
      {
        return _distributer.Distribute( request ).ProcessRequest( request );
      }
    }



    /// <summary>
    /// returns the request emitter as a pipeline handler.
    /// </summary>
    /// <param name="emitter">HTTP request distributer</param>
    /// <returns></returns>
    public static IHttpPipelineHandler AsHandler( this IHttpPipelineEmitter emitter )
    {
      return new EmitterHandler( emitter );
    }



    /// <summary>
    /// returns the accesspoint as a HTTP pipeline.
    /// </summary>
    /// <remarks>
    /// it's create a dummy pipeline object, it can not as downstream pipeline joined by anothor pipeline.
    /// </remarks>
    /// <typeparam name="T">access point type</typeparam>
    /// <param name="accessPoint">access point</param>
    /// <param name="combinedAction">action of combined</param>
    /// <returns></returns>
    public static IHttpPipeline AsPipeline<T>( this IHttpPipelineAccessPoint<T> accessPoint, Action<T> combinedAction )
    {
      return new CombinatorPipelineWrapper<T>( accessPoint, combinedAction );
    }

    private class CombinatorPipelineWrapper<T> : IDummyHttpPipeline
    {
      private readonly IHttpPipelineAccessPoint<T> _accessPoint;
      private readonly Action<T> _combinedAction;

      public CombinatorPipelineWrapper( IHttpPipelineAccessPoint<T> accessPoint, Action<T> combinedAction )
      {
        _accessPoint = accessPoint;
        _combinedAction = combinedAction;
      }

      public IHttpPipelineHandler Join( IHttpPipelineHandler downstream )
      {
        _combinedAction( Combined = _accessPoint.Combine( downstream ) );
        return null;
      }

      public override string ToString()
      {
        return _accessPoint.ToString();
      }


      public T Combined { get; private set; }


    }



    private static readonly HashSet<string> contentHeaders = new HashSet<string>( StringComparer.OrdinalIgnoreCase )
    {
      "Allow",
      "Content-Disposition",
      "Content-Encoding",
      "Content-Language",
      "Content-Length",
      "Content-Location",
      "Content-MD5",
      "Content-Range",
      "Content-Type",
      "Expires",
      "Last-Modified"
     };


    public static bool IsContentHeader( this string header )
    {
      return contentHeaders.Contains( header );
    }








    /// <summary>
    /// 接入一个管线
    /// </summary>
    /// <param name="upstream">上游管线</param>
    /// <param name="downstream">要接入的下游管线</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline Join( this IHttpPipeline upstream, IHttpPipeline downstream )
    {
      return new HttpPipelineJointer( upstream, downstream );
    }


    /// <summary>
    /// 接入一个管线
    /// </summary>
    /// <param name="upstream">上游管线</param>
    /// <param name="downstream">要接入的下游管线</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline Join( this IHttpPipeline upstream, Func<IHttpPipelineHandler, IHttpPipelineHandler> downstream )
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
      return pipeline.Join( new HttpPipelineLoadBalancer( pipelines ) );
    }

    /// <summary>
    /// 使用负载均衡器
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="pipelines">下游管线列表</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipeline UseLoadBalancer( this IHttpPipeline pipeline, params Func<IHttpPipeline, IHttpPipeline>[] pipelinesFactories )
    {
      return pipeline.Join( new HttpPipelineLoadBalancer( pipelinesFactories.Select( f => f( pipeline ) ).ToArray() ) );
    }

    /// <summary>
    /// 分发管线
    /// </summary>
    /// <param name="pipeline">上游管线</param>
    /// <param name="distributer">分发器</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipelineHandler Distribute( this IHttpPipeline pipeline, IHttpPipelineDistributer distributer )
    {
      if ( distributer == null )
        throw new ArgumentNullException( nameof( distributer ) );

      return pipeline.Join( distributer.AsHandler() );
    }

    /// <summary>
    /// 分发管线
    /// </summary>
    /// <param name="middleware">上游管线</param>
    /// <param name="pipelines">下游管线列表</param>
    /// <returns>请求处理管线</returns>
    public static IHttpPipelineHandler Distribute( this IHttpPipeline pipeline, params IHttpPipelineHandler[] pipelines )
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
    public static IHttpPipelineHandler Emit( this IHttpPipeline pipeline )
    {
      return Emit( pipeline, new HttpPipelineEmitterOptions() );
    }

    /// <summary>
    /// join pipeline with a request emitter, create a http pipeline handler.
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="handler">http message handler</param>
    /// <returns>http pipeline handler</returns>
    public static IHttpPipelineHandler Emit( this IHttpPipeline pipeline, HttpMessageHandler handler )
    {
      return Emit( pipeline, new HttpPipelineEmitterOptions( handler ) );
    }

    /// <summary>
    /// join pipeline with a request emitter, create a http pipeline handler.
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="options">emitter options</param>
    /// <returns>http pipeline handler</returns>
    public static IHttpPipelineHandler Emit( this IHttpPipeline pipeline, HttpPipelineEmitterOptions options )
    {
      return pipeline.Join( new HttpPipelineEmitter( options ).AsHandler() );
    }



    /// <summary>
    /// join pipeline with a request emitter, create a http pipeline handler.
    /// </summary>
    /// <param name="pipeline">upstream pipeline</param>
    /// <param name="emitter">request emitter</param>
    /// <returns>http pipeline handler</returns>
    public static IHttpPipelineHandler Emit( this IHttpPipeline pipeline, IHttpPipelineEmitter emitter )
    {
      return pipeline.Join( emitter.AsHandler() );
    }









    private class EmitterHandler : IHttpPipelineHandler
    {
      private readonly IHttpPipelineEmitter _emitter;

      public EmitterHandler( IHttpPipelineEmitter emitter )
      {
        _emitter = emitter;
      }

      public ValueTask<HttpResponseMessage> ProcessRequest( HttpRequestMessage request )
      {
        return _emitter.EmitRequest( request );
      }
    }

  }
}
