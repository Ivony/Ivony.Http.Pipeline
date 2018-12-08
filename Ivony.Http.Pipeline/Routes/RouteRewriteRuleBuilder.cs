using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  public class RouteRewriteRuleBuilder
  {

    internal RouteRewriteRuleBuilder( IRouteRulesBuilder builder )
    {
      Builder = builder ?? throw new ArgumentNullException( nameof( builder ) );
    }

    public IRouteRulesBuilder Builder { get; }


    private List<RouteRequestTemplate> templateList = new List<RouteRequestTemplate>();

    public RouteRewriteRuleBuilder Match( string template )
    {
      return Match( new RouteRequestTemplate( template ) );
    }

    public RouteRewriteRuleBuilder Match( RouteRequestTemplate template )
    {
      templateList.Add( template );
      return this;
    }

    public IRouteRulesBuilder Rewrite( string template )
    {
      return Rewrite( new RouteRequestTemplate( template ) );
    }


    public IRouteRulesBuilder Rewrite( RouteRequestTemplate template )
    {
      Builder.AddRule( new RouteRewriteRule( templateList, template ) );
      return Builder;
    }
  }
}
