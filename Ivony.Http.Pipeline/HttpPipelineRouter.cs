using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// implement a http piieline router.
  /// </summary>
  public class HttpPipelineRouter : IHttpPipeline
  {


    /// <summary>
    /// create a HttpPipelineRouter instance
    /// </summary>
    /// <param name="rules">route rules</param>
    public HttpPipelineRouter( params (Func<HttpRequestMessage, HttpPipelineRouteData> router, IHttpRoutePipeline pipeline)[] rules )
    {
      Rules = rules ?? throw new ArgumentNullException( nameof( rules ) );
    }

    public (Func<HttpRequestMessage, HttpPipelineRouteData> router, IHttpRoutePipeline pipeline)[] Rules { get; }

    public HttpPipelineHandler Pipe( HttpPipelineHandler handler )
    {

      var _rules = Rules.Select( rule => (Func<HttpRequestMessage, HttpPipelineHandler>) (request =>
      {
        var routeData = rule.router( request );
        if ( routeData == null )
          return null;

        else
          return r => rule.pipeline.HandleRequest( r, routeData, handler );
      }) ).ToArray();

      return new HttpPipelineConditionDispatcher( _rules, HandleExcept ).AsHandler();
    }

    private Task<HttpResponseMessage> HandleExcept( HttpRequestMessage request )
    {
      throw new NotImplementedException();
    }

    protected virtual Task<HttpResponseMessage> DefaultResponse( HttpRequestMessage request )
    {
      return Task.FromResult( new HttpResponseMessage( System.Net.HttpStatusCode.NotFound )
      {
        Content = new ByteArrayContent( new byte[0] )
      } );

    }
  }
}
