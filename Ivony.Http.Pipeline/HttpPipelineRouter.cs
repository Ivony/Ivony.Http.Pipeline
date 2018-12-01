using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
    public HttpPipelineRouter( params (Predicate<HttpRequestMessage> condition, IHttpPipeline pipeline)[] rules )
    {
      Rules = rules ?? throw new ArgumentNullException( nameof( rules ) );
    }

    public (Predicate<HttpRequestMessage> condition, IHttpPipeline pipeline)[] Rules { get; }

    public HttpPipelineHandler Pipe( HttpPipelineHandler handler )
    {

      var dispatcher = new HttpPipelineConditionDispatcher( Rules.Select( r => (r.condition, r.pipeline.Pipe( handler )) ).ToArray(), HandleExcept );
      return dispatcher.AsHandler();

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
