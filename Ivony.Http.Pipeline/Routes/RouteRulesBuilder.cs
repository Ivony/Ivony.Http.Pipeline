using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  public class RouteRulesBuilder : IRouteRulesBuilder
  {


    private List<IHttpPipelineRouteRule> _rules = new List<IHttpPipelineRouteRule>();


    public void AddRule( IHttpPipelineRouteRule rule )
    {
      _rules.Add( rule );
    }

    public IHttpPipelineRouteRule[] GetRules()
    {
      return _rules.ToArray();
    }

  }
}
