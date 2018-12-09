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

        var template = new RoutePathTemplate( "/{path*}" );
        var values = template.GetRouteValues( pathSegments );

        Assert.AreEqual( values.Count, 1 );
        Assert.AreEqual( values.Keys.First(), "path" );
        Assert.AreEqual( values.Values.First(), "News/Test/1" );
        Assert.AreEqual( values["path"], "News/Test/1" );
      }

      {
        var template = new RouteRequestTemplate( "http://163.com/news/{path*}" );
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
      var url = new Uri( "http://163.com/News/Test/1" );
      var pathSegments = PathSegments.Create( url.AbsolutePath );

      var template = new RoutePathTemplate( "/{path}" );
      var values = new Dictionary<string, string>
      {
        ["path"] = "/a/b/c"
      };

      template.Rewrite( values );
    }

    [TestMethod]
    public void PathRouteRewrite()
    {
      var rule = new RouteRewriteRule( "http://163.com/news/{path*}", "http://163.com/{path*}" );
      var request = new HttpRequestMessage( HttpMethod.Get, "http://163.com/news/Test/TestNews" );

      request = rule.Rewrite( request );
      Assert.AreEqual( request.RequestUri.AbsoluteUri, "http://163.com/Test/TestNews" );

    }
  }
}
