using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public class HttpPipelineRouter : IHttpPipeline
  {


    public HttpPipelineRouter( params HttpPipelineRouteRule[] rules )
    {
      Rules = rules ?? throw new ArgumentNullException( nameof( rules ) );
    }

    public HttpPipelineRouteRule[] Rules { get; }

    public HttpPipelineHandler Pipe( HttpPipelineHandler handler )
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
