using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Ivony.Http.Pipeline.Routes
{
  public class RewriteRequestTemplate
  {
    private static readonly string schemeRegex = @"(?<scheme>[a-zA-Z]+):";
    private static readonly string hostRegex = @"//(?<host>[^:/]+)(:(?<port>([0-9]+)|\?))?";
    private static readonly string pathRegex = @"(?<path>/((?!/)[^?]*))";
    private static readonly string queryRegex = @"\?(?<query>.+)";



    private Regex urlRegex = new Regex( $"^(?>({schemeRegex})?{hostRegex})?(?>{pathRegex})(?>{queryRegex})?$", RegexOptions.Compiled );


    public RewriteRequestTemplate( string template )
    {

      var match = urlRegex.Match( template );
      if ( match.Success == false )
        throw new FormatException();

      if ( match.Groups["scheme"].Success )
        Scheme = match.Groups["scheme"].Value;

      if ( match.Groups["host"].Success )
        HostTemplate = new RewriteHostTemplate( match.Groups["host"].Value );

      if ( match.Groups["port"].Success )
        Port = match.Groups["port"].Value;


      PathTemplate = new RewritePathTemplate( match.Groups["path"].Value );

      if ( match.Groups["query"].Success )
        QueryStringTemplate = new RewriteQueryStringTemplate( match.Groups["query"].Value );
    }


    /// <summary>
    /// 所要匹配的方案协议
    /// </summary>
    public string Scheme { get; }

    /// <summary>
    /// 路径模板
    /// </summary>
    public RewritePathTemplate PathTemplate { get; }

    /// <summary>
    /// 主机模板
    /// </summary>
    public RewriteHostTemplate HostTemplate { get; }

    /// <summary>
    /// 查询字符串模板
    /// </summary>
    public RewriteQueryStringTemplate QueryStringTemplate { get; }

    /// <summary>
    /// 端口号，如果有的话
    /// </summary>
    public string Port { get; }



    private StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;


    /// <summary>
    /// 从请求中解析出路由值
    /// </summary>
    /// <param name="request">请求数据</param>
    /// <returns>路由值</returns>
    public IReadOnlyDictionary<string, string> GetRouteValues( RouteRequestData request )
    {

      if ( Scheme != null && stringComparer.Equals( Scheme, request.Scheme ) == false )
        return null;


      var routeValues = new Dictionary<string, string>( stringComparer );

      if ( HostTemplate != null )
      {
        var values = HostTemplate.GetRouteValues( request.Host );
        if ( values == null )
          return null;

        foreach ( var (key, value) in values )
          routeValues.Add( key, value );
      }


      {
        var values = PathTemplate.GetRouteValues( request.Path );
        if ( values == null )
          return null;

        foreach ( var pair in values )
          routeValues.Add( pair.Key, pair.Value );
      }


      if ( QueryStringTemplate != null )
      {
        var values = QueryStringTemplate.GetRouteValues( request.Path );
        if ( values == null )
          return null;

        foreach ( var pair in values )
          routeValues.Add( pair.Key, pair.Value );
      }


      return routeValues;

    }


#pragma warning disable IDE0017 // 简化对象初始化
    public HttpRequestMessage RewriteRequest( HttpRequestMessage request, IReadOnlyDictionary<string, string> routeValues )
    {

      var builder = new UriBuilder( request.RequestUri );

      builder.Host = HostTemplate.Rewrite( routeValues );
      builder.Path = PathTemplate.Rewrite( routeValues );
      if ( Port == null )
        builder.Port = -1;

      else if ( Port != "?" )
        builder.Port = int.Parse( Port );


      request.RequestUri = builder.Uri;

      return request;
    }
#pragma warning restore IDE0017 // 简化对象初始化

  }
}
