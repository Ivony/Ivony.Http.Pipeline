using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline.Routes
{
  public sealed class RouteRequestData
  {


    public RouteRequestData( HttpRequestMessage request )
    {
      Path = PathSegments.Create( request.RequestUri.AbsolutePath );
      Scheme = request.RequestUri.Scheme;
      QueryString = ParseQueryString( request.RequestUri.Query );
    }


    private static readonly Regex queryStringRegex = new Regex( @"\?(\w+)=(\w+)(\&(\w+)=(\w+))*", RegexOptions.Compiled );


    private IEnumerable<(string name, string value)> ParseQueryString( string query )
    {
      if ( query == null )
        return null;


      var list = new List<(string name, string value)>();
      var name = new List<char>( 20 );
      var value = new List<char>( 50 );

      bool status = false;
      bool hasValue = false;


      for ( int i = 1; i < query.Length; i++ )
      {

        char ch = query[i];
        if ( ch == '=' )
        {
          if ( name == null )
            throw new FormatException();

          status = true;
          hasValue = true;
        }
        else if ( ch == '&' )
        {
          var n = new string( name.ToArray() );
          if ( hasValue )
            list.Add( (n, new string( value.ToArray() )) );

          else
            list.Add( (n, null) );

          status = false;
          hasValue = false;

          name = new List<char>( 20 );
          value = new List<char>( 50 );
        }
        else if ( status == false )
        {
          name.Add( ch );
        }
        else
        {
          value.Add( ch );
        }
      }

      return list;
    }

    /// <summary>
    /// 协议
    /// </summary>
    public string Scheme { get; }

    /// <summary>
    /// 路径
    /// </summary>
    public PathSegments Path { get; }

    /// <summary>
    /// 查询字符串
    /// </summary>
    public IEnumerable<(string name, string value)> QueryString { get; }

    /// <summary>
    /// HTTP 头信息
    /// </summary>
    public IReadOnlyDictionary<string, string> Headers { get; }

    /// <summary>
    /// Host 信息
    /// </summary>
    public string Host { get; }

  }
}
