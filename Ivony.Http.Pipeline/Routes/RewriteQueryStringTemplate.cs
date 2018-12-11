using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  public class RewriteQueryStringTemplate
  {

    public RewriteQueryStringTemplate( string queryString )
    {

    }

    internal IDictionary<string, string> GetRouteValues( PathSegments path )
    {
      return new Dictionary<string, string>();
    }
  }
}
