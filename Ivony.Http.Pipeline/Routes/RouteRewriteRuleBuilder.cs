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


    private List<RewriteRequestTemplate> templateList = new List<RewriteRequestTemplate>();

    public RouteRewriteRuleBuilder Match( string template )
    {
      return Match( new RewriteRequestTemplate( template ) );
    }

    public RouteRewriteRuleBuilder Match( RewriteRequestTemplate template )
    {
      templateList.Add( template );
      return this;
    }

    public IRouteRulesBuilder Rewrite( string template )
    {
      return Rewrite( new RewriteRequestTemplate( template ) );
    }


    public IRouteRulesBuilder Rewrite( RewriteRequestTemplate template )
    {
      Builder.AddRule( new RouteRewriteRule( templateList, template ) );
      return Builder;
    }
  }
}
