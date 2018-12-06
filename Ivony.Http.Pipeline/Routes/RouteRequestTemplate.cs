using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Ivony.Http.Pipeline.Routes
{
  public class RouteRequestTemplate
  {
    private static readonly string schemeRegex = @"(?<scheme>[a-zA-Z]+):";
    private static readonly string hostRegex = @"//(?<host>([a-zA-Z0-9\.\-]+)(:[0-9])?)";
    private static readonly string pathRegex = @"(?<path>[^?]*)";
    private static readonly string queryRegex = @"\?(?<query>.+)";



    private Regex urlRegex = new Regex( $"^(({schemeRegex})?{hostRegex})?{pathRegex}({queryRegex})?$", RegexOptions.Compiled );


    public RouteRequestTemplate( string template )
    {

      var match = urlRegex.Match( template );
      if ( match.Success == false )
        throw new FormatException();

      if ( match.Groups["scheme"].Success )
        Scheme = match.Groups["scheme"].Value;

      PathTemplate = new RoutePathTemplate( match.Groups["path"].Value );

      if ( match.Groups["host"].Success )
        HostTemplate = new RouteHostTemplate( match.Groups["host"].Value );

      if ( match.Groups["query"].Success )
        QueryStringTemplate = new RouteQueryStringTemplate( match.Groups["query"].Value );
    }


    /// <summary>
    /// 所要匹配的方案协议
    /// </summary>
    public string Scheme { get; }

    /// <summary>
    /// 路径模板
    /// </summary>
    public RoutePathTemplate PathTemplate { get; }

    /// <summary>
    /// 主机模板
    /// </summary>
    public RouteHostTemplate HostTemplate { get; }

    /// <summary>
    /// 查询字符串模板
    /// </summary>
    public RouteQueryStringTemplate QueryStringTemplate { get; }



    /// <summary>
    /// 从请求中解析出路由值
    /// </summary>
    /// <param name="request">请求数据</param>
    /// <returns>路由值</returns>
    public IDictionary<string, string> GetRouteValues( RouteRequestData request )
    {
      return PathTemplate.GetRouteValues( request.Path );

    }


    public HttpRequestMessage RewriteRequest( HttpRequestMessage request, IReadOnlyDictionary<string, string> routeValues )
    {

      var builder = new UriBuilder( request.RequestUri );
      builder.Path = PathTemplate.RewritePath( routeValues );

      request.RequestUri = builder.Uri;

      return request;
    }

  }
}
