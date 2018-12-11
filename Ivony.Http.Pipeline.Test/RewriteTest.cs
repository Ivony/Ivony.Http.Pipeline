using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Ivony.Http.Pipeline.Routes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ivony.Http.Pipeline.Test
{
  [TestClass]
  public class RewriteTest
  {
    private static HttpRequestMessage CreateRequest()
    {
      var url = new Uri( "http://www.163.com:8080/News/Test/1" );
      var request = new HttpRequestMessage( HttpMethod.Get, url );
      return request;
    }


    [TestMethod]
    public void Host()
    {
      var request = CreateRequest();

      var rule = new RouteRewriteRule( "/{path*}", "//www.jumony.net/{path}" );
      request = rule.Rewrite( request );

      Assert.AreEqual( request.RequestUri.AbsoluteUri, "http://www.jumony.net/News/Test/1" );

    }



    [TestMethod]
    public void Port()
    {
      var request = CreateRequest();

      var rule = new RouteRewriteRule( "/{path*}", "//www.jumony.net:1234/{path}" );
      request = rule.Rewrite( request );

      Assert.AreEqual( request.RequestUri.AbsoluteUri, "http://www.jumony.net:1234/News/Test/1" );
    }

    [TestMethod]
    public void Port2()
    {
      var request = CreateRequest();

      var rule = new RouteRewriteRule( "/{path*}", "//www.jumony.net:?/{path}" );
      request = rule.Rewrite( request );

      Assert.AreEqual( request.RequestUri.AbsoluteUri, "http://www.jumony.net:8080/News/Test/1" );
    }

    [TestMethod]
    public void HostAndPath()
    {
      var request = CreateRequest();

      var rule = new RouteRewriteRule( "//{host*}/{path*}", "//proxy.net/{host}/{path}" );
      request = rule.Rewrite( request );

      Assert.AreEqual( request.RequestUri.AbsoluteUri, "http://proxy.net/www.163.com/News/Test/1" );

    }


  }
}
