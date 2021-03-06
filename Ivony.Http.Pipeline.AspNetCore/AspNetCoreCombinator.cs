﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Ivony.Http.Pipeline
{


  /// <summary>
  /// implement a combinator to combine HTTP pipeline and asp.net core server
  /// </summary>
  public class AspNetCoreCombinator : IHttpPipelineAccessPoint<RequestDelegate>
  {
    internal static readonly string HttpContextAccessKey = "__HttpContext";
    private readonly ILogger<AspNetCoreCombinator> _logger;
    private readonly IAspNetCoreExceptionHandler _exceptionHandler;


    public AspNetCoreCombinator( IServiceProvider serviceProvider )
    {
      _logger = serviceProvider.GetRequiredService<ILogger<AspNetCoreCombinator>>();
      _exceptionHandler = serviceProvider.GetRequiredService<IAspNetCoreExceptionHandler>();
    }


    protected virtual async Task ApplyResponse( HttpContext context, HttpResponseMessage response )
    {

      context.Response.StatusCode = (int) response.StatusCode;
      context.Response.Headers.Clear();



      var ignores = response.Headers.Connection;
      var exceptions = new List<Exception>();

      ApplyHeaders( response.Headers );

      if ( response.Content == null )
        return;

      ApplyHeaders( response.Content.Headers );



      if ( exceptions.Any() )
      {
        _logger.LogError( new AggregateException( exceptions ).ToString() );
      }

      try
      {

        if ( response.StatusCode != HttpStatusCode.NoContent
          && response.StatusCode != HttpStatusCode.ResetContent
          && response.StatusCode != HttpStatusCode.NotModified
          && response.Content != null
          )
          await response.Content.CopyToAsync( context.Response.Body );
      }
      catch ( Exception e )
      {
        throw new Exception( $"apply response body failed.", e );
      }


      void ApplyHeaders( HttpHeaders headers )
      {
        foreach ( var item in headers )
        {
          if ( ignoreHeaders.Contains( item.Key ) )
            continue;

          if ( ignores.Contains( item.Key ) )
            continue;

          try
          {
            context.Response.Headers.Add( item.Key, new StringValues( item.Value.ToArray() ) );
          }
          catch ( Exception e )
          {
            exceptions.Add( new Exception( $"apply header {item.Key} failed.", e ) );

          }
        }
      }
    }


    protected virtual Task<HttpRequestMessage> CreateRequest( HttpContext context )
    {
      var request = CreateRequestCore( context );

      request.Method = new HttpMethod( context.Request.Method );
      request.RequestUri = CreateUri( context.Request );

      request.Content = new StreamContent( context.Request.Body );

      var ignores = request.Headers.Connection;

      foreach ( var item in context.Request.Headers )
      {
        if ( ignoreHeaders.Contains( item.Key ) )
          continue;

        if ( ignores.Contains( item.Key ) )
          continue;

        if ( item.Key.IsContentHeader() )
          request.Content.Headers.Add( item.Key, item.Value.AsEnumerable() );

        else
          request.Headers.Add( item.Key, item.Value.AsEnumerable() );
      }

      return Task.FromResult( request );
    }


    protected readonly static HashSet<string> ignoreHeaders = new HashSet<string>( StringComparer.OrdinalIgnoreCase ) { "Accept-Encoding", "Connection", "Content-Encoding", "Content-Length", "Keep-Alive", "Transfer-Encoding", "TE", "Accept-Transfer-Encoding", "Trailer", "Upgrade", "Proxy-Authorization", "Proxy-Authenticate" };

    protected virtual Uri CreateUri( HttpRequest request )
    {
      var builder = new UriBuilder();
      builder.Scheme = request.Scheme;
      if ( request.Host.HasValue )
      {
        builder.Host = request.Host.Host;
        if ( request.Host.Port.HasValue )
          builder.Port = request.Host.Port.Value;
      }

      builder.Path = request.Path;
      builder.Query = request.QueryString.Value;

      return builder.Uri;
    }


    private static HttpRequestMessage CreateRequestCore( HttpContext context )
    {
      var request = new HttpRequestMessage();
      request.Properties[HttpContextAccessKey] = context;
      return request;
    }

    RequestDelegate IHttpPipelineAccessPoint<RequestDelegate>.Combine( IHttpPipelineHandler pipeline )
    {
      if ( pipeline == null )
        throw new ArgumentNullException( nameof( pipeline ) );

      return async context =>
      {
        var request = await CreateRequest( context );


        HttpResponseMessage response;
        try
        {
          response = await pipeline.ProcessRequest( request, context.RequestAborted );
        }
        catch ( OperationCanceledException )
        {
          throw;
        }
        catch ( Exception e )
        {
          await _exceptionHandler.HandleExceptionAsync( context, e );
          return;
        }

        await ApplyResponse( context, response );
      };
    }
  }
}