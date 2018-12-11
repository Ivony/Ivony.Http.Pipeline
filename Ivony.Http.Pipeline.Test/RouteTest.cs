using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Ivony.Http.Pipeline.Routes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ivony.Http.Pipeline.Test
{

  [TestClass]
  public class RouteTest
  {

    [TestMethod]
    public void PathSegmentsTest()
    {

      var segments = PathSegments.Create( "/a/b/c" );
      Assert.AreEqual( segments.Count, 3 );
      Assert.AreEqual( segments[0], "a" );
      Assert.AreEqual( segments[1], "b" );
      Assert.AreEqual( segments[2], "c" );

    }


    [TestMethod]
    public void PathRoute()
    {
      var url = new Uri( "http://163.com/News/Test/1" );

      {
        var pathSegments = PathSegments.Create( url.AbsolutePath );

        var template = new RewritePathTemplate( "/{path*}" );
        var values = template.GetRouteValues( pathSegments );

        Assert.AreEqual( values.Count, 1 );
        Assert.AreEqual( values.Keys.First(), "path" );
        Assert.AreEqual( values.Values.First(), "News/Test/1" );
        Assert.AreEqual( values["path"], "News/Test/1" );
      }

      {
        var template = new RewriteRequestTemplate( "http://163.com/news/{path*}" );
        var values = template.GetRouteValues( new RouteRequestData( new HttpRequestMessage( HttpMethod.Get, url ) ) );

        Assert.AreEqual( values.Count, 1 );
        Assert.AreEqual( values.Keys.First(), "path" );
        Assert.AreEqual( values.Values.First(), "Test/1" );
        Assert.AreEqual( values["path"], "Test/1" );
      }
    }

    [TestMethod]
    public void PathRewrite()
    {
      var template = new RewritePathTemplate( "/{path}" );
      var values = new Dictionary<string, string>
      {
        ["path"] = "a/b/c"
      };

      var result = template.Rewrite( values );

      Assert.AreEqual( result, "/a/b/c" );
    }

    [TestMethod]
    public void HostRewrite()
    {
      var request = CreateRequest();

      var rule = new RouteRewriteRule( "/{path*}", "//www.jumony.net/{path}" );
      request = rule.Rewrite( request );

      Assert.AreEqual( request.RequestUri.AbsoluteUri, "http://www.jumony.net/News/Test/1" );

    }

    [TestMethod]
    public void EmptyPath()
    {
      var pathTemplate = new RewritePathTemplate( "/{path*}" );
      var values = pathTemplate.GetRouteValues( PathSegments.Create( "/" ) );
      Assert.IsTrue( values.ContainsKey( "path" ) );
      Assert.AreEqual( values["path"], null );
    }

    [TestMethod]
    public void HostAndPath()
    {
      var request = CreateRequest();

      var rule = new RouteRewriteRule( "//{host*}/{path*}", "//proxy.net/{host}/{path}" );
      request = rule.Rewrite( request );

      Assert.AreEqual( request.RequestUri.AbsoluteUri, "http://proxy.net/www.163.com/News/Test/1" );

    }

    private static HttpRequestMessage CreateRequest()
    {
      var url = new Uri( "http://www.163.com/News/Test/1" );
      var request = new HttpRequestMessage( HttpMethod.Get, url );
      return request;
    }

    [TestMethod]
    public void PathRouteRewrite()
    {
      var rule = new RouteRewriteRule( new[] { new RewriteRequestTemplate( "http://163.com/news/{path*}" ) }, new RewriteRequestTemplate( "http://163.com/{path*}" ) );
      var request = new HttpRequestMessage( HttpMethod.Get, "http://163.com/news/Test/TestNews" );

      request = rule.Rewrite( request );
      Assert.AreEqual( request.RequestUri.AbsoluteUri, "http://163.com/Test/TestNews" );

    }
  }
}
