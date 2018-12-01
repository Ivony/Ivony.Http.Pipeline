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
    /// <param name="pipelineFactory"></param>
    public HttpPipelineRouteBuilder AddRule( Func<HttpRequestMessage, HttpPipelineRouteData> router, Func<IHttpPipeline, IHttpRoutePipeline> pipelineFactory )
    {
      return AddRule( router, pipelineFactory( HttpPipeline.Blank ) );
    }

    /// <summary>
    /// 添加路由规则
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="pipeline"></param>
    public HttpPipelineRouteBuilder AddRule( Func<HttpRequestMessage, HttpPipelineRouteData> router, IHttpRoutePipeline pipeline )
    {
      rules.Add( (router, pipeline) );

      return this;
    }




    public HttpPipelineRouteBuilder MapPath( string sourcePathTemplate, string targetPathTemplate )
    {
      var source = new PathTemplate( sourcePathTemplate );
      var target = new PathTemplate( targetPathTemplate );



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



    protected ICollection<(Func<HttpRequestMessage, HttpPipelineRouteData> router, IHttpRoutePipeline pipeline)> rules = new List<(Func<HttpRequestMessage, HttpPipelineRouteData> router, IHttpRoutePipeline pipeline)>();


    public HttpPipelineRouter Build()
    {
      return new HttpPipelineRouter( rules.ToArray() );

    }





  }
}
