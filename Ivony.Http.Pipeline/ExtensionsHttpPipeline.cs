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



    public static IHttpPipelineHandler AsHandler( this IHttpPipelineEmitter emitter )
    {
      return new EmitterHandler( emitter );
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
