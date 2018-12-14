using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Ivony.Http.Pipeline.Test
{


  [TestClass]
  public class PipelineTest
  {


    [TestMethod]
    public void DymmyPipeline()
    {
      var combinator = new AspNetCoreCombinator();
      var pipeline = combinator.AsPipeline( application => { } );



      void m()
      {
        HttpPipeline.Blank.Join( pipeline );
      };

      ((Action) m).Should().Throw<InvalidOperationException>();




    }


  }
}
