using System.Net.Http;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public interface IHttpPipelineEmitter
  {
    ValueTask<HttpResponseMessage> EmitRequest( HttpRequestMessage request );
  }
}