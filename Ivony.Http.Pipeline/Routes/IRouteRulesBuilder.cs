namespace Ivony.Http.Pipeline.Routes
{
  public interface IRouteRulesBuilder
  {
    void AddRule( IHttpPipelineRouteRule rule );

    void Otherwise( IHttpPipelineHandler handler );

    IHttpPipelineRouteRule[] GetRules();

    IHttpPipelineHandler GetOtherwiseHandler();

  }
}