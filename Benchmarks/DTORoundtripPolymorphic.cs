using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using MemoryPack;
using System;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net90)]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripPolymorphic
    {
        //private readonly IDataStore DataStore = new DataFac.Storage.Testing.TestDataStore();

        [Benchmark(Baseline = true)]
        public int Polymorphic_MemoryPack()
        {
            var dto = new TestModels.MemPack.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };
            dto.Freeze();
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<TestModels.MemPack.Shape>(dto);
            var copy = MemoryPackSerializer.Deserialize<TestModels.MemPack.Shape>(buffer.Span);
            copy!.Freeze();
            return buffer.Length;
        }

        [Benchmark]
        public int Polymorphic_JsonSystemText()
        {
            var dto = new TestModels.JsonSystemText.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };
            dto.Freeze();
            string buffer = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.SerializeToJson<TestModels.JsonSystemText.Shape>(dto);
            var recdMsg = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.DeserializeFromJson<TestModels.JsonSystemText.Shape>(buffer);
            recdMsg!.Freeze();
            return buffer.Length;
        }

        [Benchmark]
        public int Polymorphic_JsonNewtonSoft()
        {
            var dto = new TestModels.JsonNewtonSoft.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };
            dto.Freeze();
            string buffer = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.SerializeToJson<TestModels.JsonNewtonSoft.Shape>(dto);
            var recdMsg = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.DeserializeFromJson<TestModels.JsonNewtonSoft.Shape>(buffer);
            recdMsg!.Freeze();
            return buffer.Length;
        }

        //[Benchmark(Baseline = true)]
        //public async ValueTask<int> Roundtrip_Polymorphic_MemBlocks()
        //{
        //    var dto = new SampleDTO.Shapes.MemBlocks.Rectangle()
        //    {
        //        Length = 3.0D,
        //        Height = 2.0D,
        //    };
        //    await dto.Pack(DataStore);
        //    var buffers = dto.GetBuffers();
        //    var copy = SampleDTO.Shapes.MemBlocks.Shape.CreateFrom(buffers);
        //    return 0;
        //}

    }
}
