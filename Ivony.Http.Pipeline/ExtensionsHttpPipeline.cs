using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ivony.Http.Pipeline.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

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




    /// <summary>
    /// get the HttpContext object
    /// </summary>
    /// <param name="request">request message</param>
    /// <returns>HttpContext object</returns>
    public static HttpContext GetHttpContext( this HttpRequestMessage request )
    {
      if ( request.Properties.TryGetValue( AspNetCoreCombinator.HttpContextAccessKey, out var value ) )
        return (HttpContext) value;

      else
        return null;
    }




  }
}
