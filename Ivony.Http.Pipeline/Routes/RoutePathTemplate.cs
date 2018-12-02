using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Ivony.Http.Pipeline.Routes
{
  public class RoutePathTemplate
  {

    Regex pathTemplateRegex = new Regex( @"(?<pathItem>[\w]\{\}+)\/+)*", RegexOptions.Compiled );

    public RoutePathTemplate( string pathTemplate )
    {
      pathTemplateRegex.Match( pathTemplate );
    }

    internal IDictionary<string, string> MatchPath( string absolutePath )
    {
      throw new NotImplementedException();
    }
  }
}
