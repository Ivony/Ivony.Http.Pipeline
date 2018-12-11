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
    public static Task<HttpResponseMessage> Handle( this IHttpPipelineDistributer distributer, HttpRequestMessage request )
    {
      return distributer.Distribute( request )( request );
    }


    /// <summary>
    /// 将管线分发器转换为处理器
    /// </summary>
    /// <param name="distributer">管线分发器</param>
    /// <returns>管线处理器</returns>
    public static HttpPipelineHandler AsHandler( this IHttpPipelineDistributer distributer )
    {
      return request => distributer.Handle( request );
    }


  }
}
