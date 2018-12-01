using System.Net.Http;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public interface IHttpRoutePipeline
  {


    Task<HttpResponseMessage> HandleRequest( HttpRequestMessage request, HttpPipelineRouteData routeData, HttpPipelineHandler handler );



  }
}