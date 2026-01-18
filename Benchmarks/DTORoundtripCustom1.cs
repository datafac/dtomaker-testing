using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Storage.Testing;
using DTOMaker.Runtime.MsgPack2;
using MemoryPack;
using System;
using System.Buffers;
using System.Threading.Tasks;

namespace Benchmarks
{
    //[SimpleJob(RuntimeMoniker.Net80)]
    //[SimpleJob(RuntimeMoniker.Net90)]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripCustom1
    {
        /// <summary>
        /// Unit tests should set this to true to validate that the values are correctly roundtripped.
        /// </summary>
        public bool CheckValues = false;

        private readonly TestDataStore _dataStore = new TestDataStore();

        [Benchmark(Baseline = true)]
        public int Roundtrip_MemoryPack()
        {
            var orig = new TestModels.MemPack.MemoryPackCustom1();
            orig.Field1 = DayOfWeek.Thursday;
            orig.Freeze();
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<TestModels.MemPack.MemoryPackCustom1>(orig);
            var copy = MemoryPackSerializer.Deserialize<TestModels.MemPack.MemoryPackCustom1>(buffer.Span);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_MsgPack2()
        {
            var dto = new TestModels.MsgPack2.Custom1();
            dto.Field1 = DayOfWeek.Thursday;
            dto.Freeze();
            ReadOnlyMemory<byte> buffer = dto.SerializeToMessagePack<TestModels.MsgPack2.Custom1>();
            var copy = buffer.DeserializeFromMessagePack<TestModels.MsgPack2.Custom1>();
            copy!.Freeze();
            if (CheckValues && !copy.Equals(dto))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public async Task<long> Roundtrip_MemBlocks()
        {
            var orig = new TestModels.MemBlocks.Custom1();
            orig.Field1 = DayOfWeek.Thursday;
            await orig.Pack(_dataStore);
            ReadOnlySequence<byte> buffers = orig.GetBuffers();
            TestModels.MemBlocks.Custom1 copy = new TestModels.MemBlocks.Custom1(buffers);
            if (CheckValues)
            {
                await copy.UnpackAll(_dataStore);
                if (!copy.Equals(orig)) throw new Exception("Roundtrip values do not match");
            }
            return buffers.Length;
        }

        [Benchmark]
        public int Roundtrip_JsonSystemText()
        {
            var orig = new TestModels.JsonSystemText.Custom1();
            orig.Field1 = DayOfWeek.Thursday;
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.SerializeToJson(orig);
            var recdMsg = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.DeserializeFromJson<TestModels.JsonSystemText.Custom1>(buffer);
            recdMsg!.Freeze();
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_JsonNewtonSoft()
        {
            var orig = new TestModels.JsonNewtonSoft.Custom1();
            orig.Field1 = DayOfWeek.Thursday;
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.SerializeToJson(orig);
            var copy = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.DeserializeFromJson<TestModels.JsonNewtonSoft.Custom1>(buffer);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }
    }
}
