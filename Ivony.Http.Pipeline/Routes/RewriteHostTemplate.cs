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
    private readonly StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;


    private string infinityKey = null;

    public RewriteHostTemplate( string hostTemplate )
    {

      var match = hostRegex.Match( hostTemplate );
      if ( match.Success == false )
        throw new FormatException();


      segments = match.Groups["segment"].Captures.Cast<Capture>().Select( capture => TemplateSegment.CreateSegment( capture.Value ) ).ToArray();

      for ( int i = 1; i < segments.Length; i++ )
      {
        if ( segments[i].Type == SegmentType.InfinityDynamic )
          throw new FormatException( "infinity dynamic segment must in the last of template" );
      }

      var set = new HashSet<string>( stringComparer );
      foreach ( var s in segments )
      {
        if ( s.Type == SegmentType.Dynamic || s.Type == SegmentType.InfinityDynamic )
        {
          if ( set.Add( s.Value ) == false )
            throw new FormatException( $"duplicate dynamic key: {s.Value}" );
        }
      }

      if ( segments[0].Type == SegmentType.InfinityDynamic )
        infinityKey = segments[0].Value;
    }



    public IEnumerable<KeyValuePair<string, string>> GetRouteValues( HostSegments host )
    {

      var result = new List<KeyValuePair<string, string>>( segments.Length );


      bool TryMatch( int length )
      {
        if ( host.Count < length )
          return false;


        for ( var i = 1; i < length + 1; i++ )
        {

          var item = segments[length - i];
          var hostItem = host[host.Count - i];
          if ( item.Type == SegmentType.Dynamic )
            result.Add( new KeyValuePair<string, string>( item.Value, hostItem ) );

          else if ( item.Type == SegmentType.Static && stringComparer.Equals( item.Value, hostItem ) == false )
            return false;
        }

        return true;

      }

      if ( infinityKey != null )
      {
        var length = segments.Length - 1;

        if ( TryMatch( length ) == false )
          return null;


        result.Add( new KeyValuePair<string, string>( infinityKey, string.Join( '.', host.Take( host.Count - length ) ) ) );
      }
      else
      {
        if ( TryMatch( segments.Length ) == false )
          return null;
      }

      return result;

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


    public override string ToString()
    {
      return string.Join( '.', segments );
    }


  }
}
