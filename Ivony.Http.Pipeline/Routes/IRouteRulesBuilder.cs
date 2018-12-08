namespace Ivony.Http.Pipeline.Routes
{
  public interface IRouteRulesBuilder
  {
    void AddRule( IHttpPipelineRouteRule rule );
    IHttpPipelineRouteRule[] GetRules();
  }
}