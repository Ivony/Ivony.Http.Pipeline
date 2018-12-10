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
      _upstream = upstream;
      _downstream = downstream;
    }

    HttpPipelineHandler IHttpPipeline.Join( HttpPipelineHandler handler )
    {
      return _upstream.Join( _downstream.Join( handler ) );
    }


    public override string ToString()
    {
      return $"{_upstream}\n-> {_downstream}";
    }

  }
}