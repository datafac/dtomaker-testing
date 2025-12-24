using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DTOMaker.Runtime.MsgPack2;
using MemoryPack;
using System;

namespace Benchmarks
{
    //[SimpleJob(RuntimeMoniker.Net80)]
    //[SimpleJob(RuntimeMoniker.Net90)]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripPolymorphic
    {
        /// <summary>
        /// Unit tests should set this to true to validate that the values are correctly roundtripped.
        /// </summary>
        public bool CheckValues = false;

        //private readonly IDataStore DataStore = new DataFac.Storage.Testing.TestDataStore();

        [Benchmark(Baseline = true)]
        public int Polymorphic_MemoryPack()
        {
            var orig = new TestModels.MemPack.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };
            orig.Freeze();
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<TestModels.MemPack.Shape>(orig);
            var copy = MemoryPackSerializer.Deserialize<TestModels.MemPack.Shape>(buffer.Span);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public int Polymorphic_MsgPack2()
        {
            var orig = new TestModels.MsgPack2.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };
            orig.Freeze();
            ReadOnlyMemory<byte> buffer = orig.SerializeToMessagePack<TestModels.MsgPack2.Shape>();
            var copy = buffer.DeserializeFromMessagePack<TestModels.MsgPack2.Shape>();
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public int Polymorphic_JsonSystemText()
        {
            var orig = new TestModels.JsonSystemText.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.SerializeToJson<TestModels.JsonSystemText.Shape>(orig);
            var copy = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.DeserializeFromJson<TestModels.JsonSystemText.Shape>(buffer);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public int Polymorphic_JsonNewtonSoft()
        {
            var orig = new TestModels.JsonNewtonSoft.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.SerializeToJson<TestModels.JsonNewtonSoft.Shape>(orig);
            var copy = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.DeserializeFromJson<TestModels.JsonNewtonSoft.Shape>(buffer);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
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
        //    if (CheckValues && !copy.Equals(orig))
        //        throw new Exception("Roundtrip values do not match");
        //    return 0;
        //}

    }
}
