using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{
  /// <summary>
  /// 路由表构建帮助程序
  /// </summary>
  public class HttpPipelineRouteBuilder
  {

    /// <summary>
    /// 添加路由规则
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="pipeline"></param>
    public HttpPipelineRouteBuilder AddRule( IHttpPipelineRouteRule routeRule )
    {
      rules.Add( routeRule );

      return this;
    }




    public HttpPipelineRouteBuilder MapPath( string sourcePathTemplate, string targetPathTemplate )
    {

      return AddRule( new PathTemplateRewriteRule( sourcePathTemplate, targetPathTemplate ) );



    }


    private class PathTemplate
    {
      public PathTemplate( string pathTemplate )
      {
      }

      public bool IsMatch( string path, out IReadOnlyDictionary<string, string> templateData )
      {
        templateData = null;
        return false;
      }

      public string ApplyPath( IReadOnlyDictionary<string, string> templateData )
      {
        return null;
      }


    }



    protected ICollection<IHttpPipelineRouteRule> rules = new List<IHttpPipelineRouteRule>();


    public HttpPipelineRouteService Build()
    {
      return new HttpPipelineRouteService( rules.ToArray() );

    }





  }
}
