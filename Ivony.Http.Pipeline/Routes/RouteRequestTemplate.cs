using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Ivony.Http.Pipeline.Routes
{
  public class RouteRequestTemplate
  {
    private static readonly string schemeRegex = @"(?>(?<scheme>[a-zA-Z]+):)?(?#schema)";
    private static readonly string hostRegex = @"(?>//(?<host>[a-zA-Z0-9\.\-]+)(:(?<port>[0-9]))?)?(?#host & port)";
    private static readonly string pathRegex = @"(?>(?<path>[^?]*))(?#path)";
    private static readonly string queryRegex = @"(?>\?(?<query>.+))?";



    private Regex urlRegex = new Regex( "^" + schemeRegex + hostRegex + pathRegex + queryRegex + "$", RegexOptions.Compiled );


    public RouteRequestTemplate( string template )
    {

      var match = urlRegex.Match( template );
      if ( match.Success == false )
        throw new FormatException();

      RoutePathTemplate = new RoutePathTemplate( match.Groups["path"].Value );
    }

    public RoutePathTemplate RoutePathTemplate { get; }



    public IDictionary<string, string> GetRouteValues( RouteRequestData request )
    {
      return RoutePathTemplate.GetRouteValues( request.Path );

    }


    public HttpRequestMessage RewriteRequest( HttpRequestMessage request, IReadOnlyDictionary<string, string> routeValues )
    {

      var builder = new UriBuilder( request.RequestUri );
      builder.Path = RoutePathTemplate.RewritePath( routeValues );

      request.RequestUri = builder.Uri;

      return request;
    }

  }
}
