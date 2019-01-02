using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ivony.Http.Pipeline
{
  public class HttpPipelineEmitterOptions
  {


    public HttpPipelineEmitterOptions( HttpClient client, bool? chunked, bool? connection )
    {
      if ( client == null )
        throw new ArgumentNullException( nameof( client ) );
      HttpClient = client;
    }


    public HttpClient HttpClient { get; }


    public bool? TransferEncodingChunked { get; }


    public bool? ConnectionClose { get; }

  }
}
