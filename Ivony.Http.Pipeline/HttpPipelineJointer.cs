using System;

namespace Ivony.Http.Pipeline
{

  /// <summary>
  /// a jointer to join two pipeline
  /// </summary>
  internal sealed class HttpPipelineJointer : IHttpPipeline
  {
    private readonly IHttpPipeline _upstream;
    private readonly IHttpPipeline _downstream;

    public HttpPipelineJointer( IHttpPipeline upstream, IHttpPipeline downstream )
    {
      if ( downstream is IDummyHttpPipeline )
        throw new InvalidOperationException( "can not join a dummy pipeline as downstream!" );
      
      _upstream = upstream;
      _downstream = downstream;
    }

    IHttpPipelineHandler IHttpPipeline.Join( IHttpPipelineHandler handler )
    {
      return _upstream.Join( _downstream.Join( handler ) );
    }


    public override string ToString()
    {
      return $"{_upstream}\n-> {_downstream}";
    }

  }
}