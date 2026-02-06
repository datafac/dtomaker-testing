using BenchmarkDotNet.Running;

namespace Benchmarks
{

    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<DTORoundtripPolymorphic>();
            //var summary = BenchmarkRunner.Run<DTORoundtripBasics>();
            //var summary = BenchmarkRunner.Run<DTORoundtripCustom1>();
            var summary = BenchmarkRunner.Run<DTORoundtripBinaryTree>();
        }
    }
}
