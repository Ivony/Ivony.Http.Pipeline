using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  public class RouteRequestTemplate
  {

    public RouteRequestTemplate( string template )
    {
      RoutePathTemplate = new RoutePathTemplate( template );
    }

    public RoutePathTemplate RoutePathTemplate { get; }


    public IDictionary<string, string> MatchRequest( HttpRequestMessage request )
    {
      return RoutePathTemplate.MatchPath( request.RequestUri.AbsolutePath );
    }

  }
}
