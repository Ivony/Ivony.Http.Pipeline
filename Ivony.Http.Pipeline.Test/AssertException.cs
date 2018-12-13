using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ivony.Http.Pipeline.Test
{
  public static class AssertException
  {

    public static void MustThrow<T>( Action action ) where T : Exception
    {
      try
      {
        action();
      }
      catch ( T )
      {
        return;
      }
      catch ( Exception e )
      {
        throw new AssertFailedException( "unintended Exception", e );
      }
      Assert.Fail( "did not throw desired Exception" );
    }

  }
}
