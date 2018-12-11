using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ivony.Http.Pipeline.Routes
{
  public class RewriteHostTemplate
  {

    private readonly static Regex hostRegex = new Regex( @"^(?<segment>([\w-]+)|(\{[\w-*]+\}))(\.(?<segment>([\w-]+)|(\{[\w-*]+\})))*$" );


    private readonly TemplateSegment[] segments;
    private string port;


    public RewriteHostTemplate( string hostTemplate )
    {

      var match = hostRegex.Match( hostTemplate );
      if ( match.Success == false )
        throw new FormatException();


      segments = match.Groups["segment"].Captures.Cast<Capture>().Select( capture => TemplateSegment.CreateSegment( capture.Value ) ).ToArray();
      if ( match.Groups["port"].Success )
        port = match.Groups["port"].Value;


    }



    public IDictionary<string, string> GetRouteValues( string hostString )
    {
      return new Dictionary<string, string>();
    }


    public string Rewrite( IReadOnlyDictionary<string, string> routeValues )
    {

      var flag = false;
      var builder = new StringBuilder( segments.Length * 10 );
      foreach ( var segment in segments )
      {

        if ( flag == false )
          flag = true;
        else
          builder.Append( '.' );

        switch ( segment.Type )
        {
          case SegmentType.Static:
            builder.Append( segment.Value );
            break;
          case SegmentType.Dynamic:
          case SegmentType.InfinityDynamic:

            if ( routeValues.TryGetValue( segment.Value, out var value ) == false )
              throw new InvalidOperationException();

            else
              builder.Append( value );

            break;
        }
      }

      return builder.ToString();

    }


  }
}
