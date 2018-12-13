using System;
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
        _combinedAction( _accessPoint.Combine( downstream ) );
        return null;
      }

      public override string ToString()
      {
        return _accessPoint.ToString();
      }

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
