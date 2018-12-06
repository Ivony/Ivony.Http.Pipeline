using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ivony.Http.Pipeline.Routes
{
  public class PathSegments : IReadOnlyList<string>
  {


    private static Regex regex = new Regex( @"^(\/+(?<segment>[\w]+))*(\/*)$" );
    private readonly string[] _segments;

    public PathSegments( string[] segments )
    {
      _segments = segments;

      generator = new Lazy<string>( () => '/' + string.Join( '/', _segments ) );
    }



    /// <summary>
    /// 解析路径创建 PathSegments 对象
    /// </summary>
    /// <param name="absoluatePath">绝对路径</param>
    /// <returns></returns>
    public static PathSegments Create( string absoluatePath )
    {

      var match = regex.Match( absoluatePath );
      if ( match.Success == false )
        throw new FormatException();


      return new PathSegments( match.Groups["segment"].Captures.Cast<Capture>().Select( capture => capture.Value ).ToArray() );

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
