using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  public class RouteRulesBuilder : IRouteRulesBuilder
  {


    private List<IHttpPipelineRouteRule> _rules = new List<IHttpPipelineRouteRule>();


    /// <summary>
    /// 添加路由规则
    /// </summary>
    /// <param name="rule">路由规则</param>
    public void AddRule( IHttpPipelineRouteRule rule )
    {
      _rules.Add( rule );
    }


    private IHttpPipelineHandler _otherwiseHandler;

    /// <summary>
    /// 设置默认情况处理器（当所有路由都不匹配的情况）
    /// </summary>
    /// <param name="handler">处理器</param>
    public void Otherwise( IHttpPipelineHandler handler )
    {
      if ( _otherwiseHandler != null )
        throw new InvalidOperationException();

      _otherwiseHandler = handler;
        
    }


    /// <summary>
    /// 获取路由规则列表
    /// </summary>
    /// <returns>路由规则列表</returns>
    public IHttpPipelineRouteRule[] GetRules()
    {
      return _rules.ToArray();
    }


    /// <summary>
    /// 获取默认情况处理器
    /// </summary>
    /// <returns>默认情况处理器</returns>
    public IHttpPipelineHandler GetOtherwiseHandler()
    {
      return _otherwiseHandler;
    }
  }
}
