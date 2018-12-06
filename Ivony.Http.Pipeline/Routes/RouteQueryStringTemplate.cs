using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  public class RouteQueryStringTemplate
  {

    public RouteQueryStringTemplate( string queryString )
    {

    }

    internal IDictionary<string, string> GetRouteValues( PathSegments path )
    {
      return new Dictionary<string, string>();
    }
  }
}
