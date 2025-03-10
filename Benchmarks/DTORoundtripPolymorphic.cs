﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Storage;
using MessagePack;
using System.Threading.Tasks;

namespace Benchmarks
{
    //[SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripPolymorphic
    {
        private readonly IDataStore DataStore = new DataFac.Storage.Testing.TestDataStore();

        [Benchmark(Baseline = true)]
        public int Roundtrip_Polymorphic_MessagePack()
        {
            var dto = new SampleDTO.Shapes.MessagePack.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };
            dto.Freeze();
            var buffer = MessagePackSerializer.Serialize<SampleDTO.Shapes.MessagePack.Shape>(dto);
            var copy = MessagePackSerializer.Deserialize<SampleDTO.Shapes.MessagePack.Shape>(buffer, out int bytesRead);
            dto.Freeze();
            return 0;
        }

        [Benchmark]
        public async ValueTask<int> Roundtrip_Polymorphic_MemBlocks()
        {
            var dto = new SampleDTO.Shapes.MemBlocks.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };
            await dto.Pack(DataStore);
            var buffers = dto.GetBuffers();
            var copy = SampleDTO.Shapes.MemBlocks.Shape.CreateFrom(buffers);
            return 0;
        }

    }
}
