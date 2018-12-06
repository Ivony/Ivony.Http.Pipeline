using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ivony.Http.Pipeline.Routes
{
  public class RoutePathTemplate
  {

    private static readonly Regex pathTemplateRegex = new Regex( @"^(\/((?<segment>[\w]+)|(?<segment>\{[\w\*]+\})))*$", RegexOptions.Compiled );



    private TemplateSegment[] segments;


    public RoutePathTemplate( string pathTemplate )
    {
      var match = pathTemplateRegex.Match( pathTemplate );
      if ( match.Success == false )
        throw new FormatException();


      segments = match.Groups["segment"].Captures.Cast<Capture>().Select( capture => TemplateSegment.CreateSegment( capture.Value ) ).ToArray();

      for ( int i = 0; i < segments.Length - 1; i++ )
      {
        if ( segments[i].Type == SegmentType.InfinityDynamic )
          throw new FormatException( "infinity dynamic segment must in the last of template" );
      }

      var set = new HashSet<string>( StringComparer.FromComparison( stringComparison ) );
      foreach ( var s in segments )
      {
        if ( s.Type == SegmentType.Dynamic || s.Type == SegmentType.InfinityDynamic )
        {
          if ( set.Add( s.Value ) == false )
            throw new FormatException( $"duplicate dynamic key: {s.Value}" );
        }
      }


    }

    public string RewritePath( IReadOnlyDictionary<string, string> routeValues )
    {

      var builder = new StringBuilder();
      foreach ( var segment in segments )
      {
        switch ( segment.Type )
        {
          case SegmentType.Static:
            builder.Append( "/" + segment.Value );
            break;
          case SegmentType.Dynamic:
          case SegmentType.InfinityDynamic:

            if ( routeValues.TryGetValue( segment.Value, out var value ) == false )
              throw new InvalidOperationException();

            else
              builder.Append( "/" + value );

            break;
        }
      }

      return builder.ToString();

    }


    private readonly StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;

    public IDictionary<string, string> GetRouteValues( PathSegments pathSegments )
    {

      var values = new Dictionary<string, string>();


      var i = 0;
      foreach ( var segment in pathSegments )
      {
        var template = segments[i];
        switch ( template.Type )
        {
          case SegmentType.Static:
            if ( template.Value.Equals( segment, stringComparison ) == false )
              return null;

            i++;
            break;

          case SegmentType.Dynamic:
            values[template.Value] = segment;

            i++;
            break;

          case SegmentType.InfinityDynamic:
            if ( values.ContainsKey( template.Value ) == false )
              values[template.Value] = segment;

            else
              values[template.Value] += "/" + segment;

            break;
        }
      }

      if ( i < segments.Length - 1 )
        return null;

      return values;
    }
  }
}