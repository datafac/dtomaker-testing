using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Memory;
using DataFac.Storage;
using DataFac.Storage.Testing;
using DTOMaker.Runtime.MsgPack2;
using MemoryPack;
using System;
using System.Buffers;
using System.Text;
using System.Threading.Tasks;
using TestModels;

namespace Benchmarks
{
    //[SimpleJob(RuntimeMoniker.Net80)]
    //[SimpleJob(RuntimeMoniker.Net90)]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripBasics
    {
        /// <summary>
        /// Unit tests should set this to true to validate that the values are correctly roundtripped.
        /// </summary>
        public bool CheckValues = false;

        //[Params(ValueKind.StringNull, ValueKind.StringEmpty, ValueKind.StringSmall, ValueKind.StringLarge)]
        [Params(ValueKind.AllPropsSet)]
        public ValueKind Kind;

        private readonly TestDataStore DataStore = new TestDataStore();

        private static readonly Guid guidValue = new("cc8af561-5172-43e6-8090-5dc1b2d02e07");

        private static readonly PairOfInt16 pairOfInt16Value = new PairOfInt16(1, -1);
        private static readonly PairOfInt32 pairOfInt32Value = new PairOfInt32(1, -1);
        private static readonly PairOfInt64 pairOfInt64Value = new PairOfInt64(1, -1);

        private static readonly string SmallString = new string('a', 32);
        private static readonly string LargeString = new string('z', 1024);

        private static readonly Octets SmallOctets = new Octets(Encoding.UTF8.GetBytes(new string('a', 32)));
        private static readonly Octets LargeOctets = new Octets(Encoding.UTF8.GetBytes(new string('z', 1024)));

        private void SetField(IMyDTO dto, ValueKind id)
        {
            switch (Kind)
            {
                case ValueKind.Bool:
                    dto.Field01 = true;
                    break;
                case ValueKind.DoubleLE:
                    dto.Field02LE = Double.MaxValue;
                    break;
                case ValueKind.Guid:
                    dto.Field04 = guidValue;
                    break;
                case ValueKind.PairOfInt16:
                    dto.Field07 = pairOfInt16Value;
                    break;
                case ValueKind.PairOfInt32:
                    dto.Field08 = pairOfInt32Value;
                    break;
                case ValueKind.PairOfInt64:
                    dto.Field09 = pairOfInt64Value;
                    break;
                case ValueKind.StringNull:
                    dto.Field05 = null;
                    break;
                case ValueKind.StringEmpty:
                    dto.Field05 = string.Empty;
                    break;
                case ValueKind.StringSmall:
                    dto.Field05 = SmallString;
                    break;
                case ValueKind.StringLarge:
                    dto.Field05 = LargeString;
                    break;
                case ValueKind.BinaryNull:
                    dto.Field06 = null;
                    break;
                case ValueKind.BinaryEmpty:
                    dto.Field06 = Octets.Empty;
                    break;
                case ValueKind.BinarySmall:
                    dto.Field06 = SmallOctets;
                    break;
                case ValueKind.BinaryLarge:
                    dto.Field06 = LargeOctets;
                    break;
                case ValueKind.AllPropsSet:
                    dto.Field01 = true;
                    dto.Field02LE = Double.MaxValue;
                    dto.Field04 = guidValue;
                    dto.Field07 = pairOfInt16Value;
                    dto.Field08 = pairOfInt32Value;
                    dto.Field09 = pairOfInt64Value;
                    dto.Field05 = SmallString;
                    dto.Field06 = SmallOctets;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), id, null);
            }
        }

        [Benchmark(Baseline = true)]
        public int Roundtrip_MemoryPack()
        {
            var orig = new TestModels.MemPack.MemoryPackMyDTO();
            SetField(orig, Kind);
            orig.Freeze();
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<TestModels.MemPack.MemoryPackMyDTO>(orig);
            var copy = MemoryPackSerializer.Deserialize<TestModels.MemPack.MemoryPackMyDTO>(buffer.Span);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_MsgPack2()
        {
            var dto = new TestModels.MsgPack2.MyDTO();
            SetField(dto, Kind);
            dto.Freeze();
            ReadOnlyMemory<byte> buffer = dto.SerializeToMessagePack<TestModels.MsgPack2.MyDTO>();
            var copy = buffer.DeserializeFromMessagePack<TestModels.MsgPack2.MyDTO>();
            copy!.Freeze();
            if (CheckValues && !copy.Equals(dto))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public async Task<long> Roundtrip_MemBlocksAsync()
        {
            var orig = new TestModels.MemBlocks.MyDTO();
            SetField(orig, Kind);
            await orig.Pack(DataStore);
            ReadOnlySequence<byte> buffers = orig.GetBuffers();
            TestModels.MemBlocks.MyDTO copy = new TestModels.MemBlocks.MyDTO(buffers);
            if (CheckValues)
            {
                await copy.UnpackAll(DataStore);
                if (!copy.Equals(orig)) throw new Exception("Roundtrip values do not match");
            }
            return buffers.Length;
        }

        [Benchmark]
        public int Roundtrip_JsonSystemText()
        {
            var orig = new TestModels.JsonSystemText.MyDTO();
            SetField(orig, Kind);
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.SerializeToJson(orig);
            var recdMsg = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.DeserializeFromJson<TestModels.JsonSystemText.MyDTO>(buffer);
            recdMsg!.Freeze();
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_JsonNewtonSoft()
        {
            var orig = new TestModels.JsonNewtonSoft.MyDTO();
            SetField(orig, Kind);
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.SerializeToJson(orig);
            var copy = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.DeserializeFromJson<TestModels.JsonNewtonSoft.MyDTO>(buffer);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }
    }
}
