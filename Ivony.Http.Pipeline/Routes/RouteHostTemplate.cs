using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ivony.Http.Pipeline.Routes
{
  public class RouteHostTemplate
  {

    private readonly static Regex hostRegex = new Regex( @"^(?<segment>([\w-]+)|(\{[\w-*]+\}))(\.(?<segment>([\w-]+)|(\{[\w-*]+\})))*$" );


    private readonly TemplateSegment[] segments;


    public RouteHostTemplate( string hostTemplate )
    {

      var match = hostRegex.Match( hostTemplate );
      if ( match.Success == false )
        throw new FormatException();


      segments = match.Groups["segment"].Captures.Cast<Capture>().Select( capture => TemplateSegment.CreateSegment( capture.Value ) ).ToArray();


    }



    public IDictionary<string, string> GetRouteValues( string hostString )
    {
      throw new NotImplementedException();
    }


  }
}
