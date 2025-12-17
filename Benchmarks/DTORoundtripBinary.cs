using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Memory;
using DataFac.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Benchmarks
{
    //[SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripBinary
    {
        [Params(ValueKind.BinaryNull, ValueKind.BinaryZero, ValueKind.BinarySmall, ValueKind.BinaryLarge)]
        public ValueKind Kind;

        private static readonly IDataStore DataStore = new DataFac.Storage.Testing.TestDataStore();

        private static readonly Octets SmallBinaryValue = new Octets(Enumerable.Range(0, 60).Select(i => (byte)i).ToArray());
        private static readonly Octets LargeBinaryValue = new Octets(Enumerable.Range(0, 256).Select(i => (byte)i).ToArray());

        //[Benchmark(Baseline = true)]
        //public async ValueTask<int> Roundtrip_MemBlocks()
        //{
        //    var dto = MakeMyDTO_MemBlocks(Kind);
        //    await dto.Pack(DataStore);
        //    var buffers = dto.GetBuffers();
        //    var copy = SampleDTO.Binary.MemBlocks.BinaryDTO.CreateFrom(buffers);
        //    return 0;
        //}
    }
}