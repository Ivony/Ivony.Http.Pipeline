using System;
using System.Collections.Generic;
using System.Text;
using Ivony.Http.Pipeline.Routes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ivony.Http.Pipeline.Test
{

  [TestClass]
  public class RewriteTemplate
  {

    [TestMethod]
    public void Host()
    {
      var template = new RewriteRequestTemplate( "//163.com/{path*}" );
      Assert.AreEqual( template.HostTemplate.ToString(), "163.com" );
      Assert.AreEqual( template.PathTemplate.ToString(), "{path*}" );
    }


    [TestMethod]
    public void Host2()
    {
      var template = new RewriteRequestTemplate( "//{host}.163.com/{path*}" );
      Assert.AreEqual( template.HostTemplate.ToString(), "{host}.163.com" );
      Assert.AreEqual( template.PathTemplate.ToString(), "{path*}" );
    }

  }
}
