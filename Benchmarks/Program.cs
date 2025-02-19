using BenchmarkDotNet.Running;

namespace Benchmarks
{

    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<DTORoundtripPolymorphic>();
            var summary = BenchmarkRunner.Run<DTORoundtripStrings>();
            //var summary = BenchmarkRunner.Run<DTORoundtripBasics>();
        }
    }
}
