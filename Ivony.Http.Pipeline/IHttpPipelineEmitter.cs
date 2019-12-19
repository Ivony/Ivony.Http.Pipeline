using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Http.Pipeline
{
  public interface IHttpPipelineEmitter
  {
    ValueTask<HttpResponseMessage> EmitRequest( HttpRequestMessage request, CancellationToken cancellationToken );
  }
}