using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Memory;
using DataFac.Storage.Testing;
using DTOMaker.Runtime.MsgPack2;
using MemoryPack;
using System;
using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestModels;

namespace Benchmarks
{

    //[SimpleJob(RuntimeMoniker.Net80)]
    //[SimpleJob(RuntimeMoniker.Net90)]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripString
    {
        /// <summary>
        /// Unit tests should set this to true to validate that the values are correctly roundtripped.
        /// </summary>
        public bool CheckValues = false;

        [Params(ValueKind.StringNull, ValueKind.StringEmpty, ValueKind.StringSmall, ValueKind.StringLarge)]
        //[Params(ValueKind.AllPropsSet)]
        //[Params(ValueKind.BinaryNull, ValueKind.BinaryEmpty, ValueKind.BinarySmall, ValueKind.BinaryLarge)]
        public ValueKind Kind;

        private readonly TestBlobStore _blobStore = new TestBlobStore();

        private static readonly Guid guidValue = new("cc8af561-5172-43e6-8090-5dc1b2d02e07");

        private static readonly PairOfInt16 pairOfInt16Value = new PairOfInt16(1, -1);
        private static readonly PairOfInt32 pairOfInt32Value = new PairOfInt32(1, -1);
        private static readonly PairOfInt64 pairOfInt64Value = new PairOfInt64(1, -1);

        private static readonly string SmallString = new string('a', 32);
        private static readonly string LargeString = new string('z', 1024);

        private static readonly Octets SmallOctets = new Octets(Encoding.UTF8.GetBytes(new string('a', 32)));
        private static readonly Octets LargeOctets = new Octets(Encoding.UTF8.GetBytes(new string('z', 1024)));

        private static string? GetString(ValueKind id)
        {
            switch (id)
            {
                case ValueKind.StringNull:
                    return null;
                case ValueKind.StringEmpty:
                    return string.Empty;
                case ValueKind.StringSmall:
                    return SmallString;
                case ValueKind.StringLarge:
                    return LargeString;
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), id, null);
            }
        }

        [Benchmark(Baseline = true)]
        public int Roundtrip_MemoryPack()
        {
            var orig = new TestModels.MemPack.MemoryPackStringDTO();
            orig.Field1 = GetString(Kind);
            orig.Freeze();
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<TestModels.MemPack.MemoryPackStringDTO>(orig);
            var copy = MemoryPackSerializer.Deserialize<TestModels.MemPack.MemoryPackStringDTO>(buffer.Span);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_MsgPack2()
        {
            var orig = new TestModels.MsgPack2.StringDTO();
            orig.Field1 = GetString(Kind);
            orig.Freeze();
            ReadOnlyMemory<byte> buffer = orig.SerializeToMessagePack<TestModels.MsgPack2.StringDTO>();
            var copy = buffer.DeserializeFromMessagePack<TestModels.MsgPack2.StringDTO>();
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public async Task<long> Roundtrip_MemBlocks()
        {
            var ct = CancellationToken.None;
            var orig = new TestModels.MemBlocks.StringDTO();
            orig.Field1 = GetString(Kind);
            await orig.Pack(_blobStore, ct);
            var buffer = await orig.Serialize(ct);
            TestModels.MemBlocks.StringDTO copy = new TestModels.MemBlocks.StringDTO(buffer);
            if (CheckValues)
            {
                await copy.UnpackAll(_blobStore, ct);
                if (!copy.Equals(orig)) throw new Exception("Roundtrip values do not match");
            }
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_JsonSystemText()
        {
            var orig = new TestModels.JsonSystemText.StringDTO();
            orig.Field1 = GetString(Kind);
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.SerializeToJson(orig);
            var recdMsg = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.DeserializeFromJson<TestModels.JsonSystemText.StringDTO>(buffer);
            recdMsg!.Freeze();
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_JsonNewtonSoft()
        {
            var orig = new TestModels.JsonNewtonSoft.StringDTO();
            orig.Field1 = GetString(Kind);
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.SerializeToJson(orig);
            var copy = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.DeserializeFromJson<TestModels.JsonNewtonSoft.StringDTO>(buffer);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }
    }
}
