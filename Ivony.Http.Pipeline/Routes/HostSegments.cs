using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ivony.Http.Pipeline.Routes
{
  public class HostSegments : IReadOnlyList<string>
  {

    private static Regex regex = new Regex( @"^(?<segment>[\w]+)(\.(?<segment>[\w]+))*$" );
    private readonly string[] _segments;

    public HostSegments( string[] segments )
    {
      _segments = segments;

      generator = new Lazy<string>( () => string.Join( '.', _segments ) );
    }



    public static HostSegments Create( string hostString )
    {

      var match = regex.Match( hostString );
      if ( match.Success == false )
        throw new FormatException();


      return new HostSegments( match.Groups["segment"].Captures.Cast<Capture>().Select( capture => capture.Value ).ToArray() );

    }


    public string this[int index] => _segments[index];

    public int Count => _segments.Length;


    public IEnumerator<string> GetEnumerator()
    {
      return ((IReadOnlyList<string>) _segments).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _segments.GetEnumerator();
    }

    private Lazy<string> generator;

    public override string ToString()
    {
      return generator.Value;
    }


  }
}
