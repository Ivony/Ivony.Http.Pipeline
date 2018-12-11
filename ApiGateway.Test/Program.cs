using System;
using System.Diagnostics;

namespace ApiGateway.Test
{
  class Program
  {

    private interface ITest
    {
      int DoSomething( int a, int b );
    }

    private class Test : ITest
    {
      public int DoSomething( int a, int b )
      {
        return unchecked(a * b);
      }
    }

    private static readonly Random random = new Random();

    private static int size = 1_000_000_000;


    static void Main( string[] args )
    {


      int[] array1 = new int[size];
      int[] array2 = new int[size];

      for ( int i = 0; i < size; i++ )
      {
        array1[i] = random.Next();
        array2[i] = random.Next();
      }


      var instance = (ITest) new Test();
      var method = (Func<int, int, int>) instance.DoSomething;

      for ( int i = 0; i < 1_000_000; i++ )
        instance.DoSomething( array1[i], array2[i] );

      for ( int i = 0; i < 1_000_000; i++ )
        method( array1[i], array2[i] );

      Console.WriteLine( "benchmark start." );


      Stopwatch watch = new Stopwatch();
      watch.Start();

      for ( int i = 0; i < size; i++ )
        instance.DoSomething( array1[i], array2[i] );

      watch.Stop();

      Console.WriteLine( watch.ElapsedMilliseconds );


      watch = new Stopwatch();
      watch.Start();

      for ( int i = 0; i < size; i++ )
        method( array1[i], array2[i] );

      watch.Stop();

      Console.WriteLine( watch.ElapsedMilliseconds );



    }
  }
}
