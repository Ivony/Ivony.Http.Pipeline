using System.Net.Http;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public interface IHttpPipelineEmitter
  {
    Task<HttpResponseMessage> EmitRequest( HttpRequestMessage request );
  }
}