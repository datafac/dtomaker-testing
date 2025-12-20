using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Memory;
using DataFac.Storage;
using MemoryPack;
using TestModels;
using System;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net90)]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripBasics
    {
        [Params(ValueKind.StringFull)]
        public ValueKind Kind;

        private readonly IDataStore DataStore = new DataFac.Storage.Testing.TestDataStore();

        private static readonly Guid guidValue = new("cc8af561-5172-43e6-8090-5dc1b2d02e07");

        private static readonly PairOfInt16 pairOfInt16Value = new PairOfInt16((Int16)1, (Int16)(-1));
        private static readonly PairOfInt32 pairOfInt32Value = new PairOfInt32((Int32)1, (Int32)(-1));
        private static readonly PairOfInt64 pairOfInt64Value = new PairOfInt64((Int64)1, (Int64)(-1));

        private static readonly string StringWith128Chars =
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef";

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
                case ValueKind.StringZero:
                    dto.Field05 = string.Empty;
                    break;
                case ValueKind.StringFull:
                    dto.Field05 = StringWith128Chars;
                    break;
                default:
                    break;
            }
        }

        [Benchmark(Baseline = true)]
        public int Roundtrip_MemoryPack()
        {
            var dto = new TestModels.MemPack.MemoryPackMyDTO();
            SetField(dto, Kind);
            dto.Freeze();
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<TestModels.MemPack.MemoryPackMyDTO>(dto);
            var copy = MemoryPackSerializer.Deserialize<TestModels.MemPack.MemoryPackMyDTO>(buffer.Span);
            copy!.Freeze();
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_JsonSystemText()
        {
            var dto = new TestModels.JsonSystemText.MyDTO();
            SetField(dto, Kind);
            dto.Freeze();
            string buffer = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.SerializeToJson(dto);
            var recdMsg = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.DeserializeFromJson<TestModels.JsonSystemText.MyDTO>(buffer);
            recdMsg!.Freeze();
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_JsonNewtonSoft()
        {
            var dto = new TestModels.JsonNewtonSoft.MyDTO();
            SetField(dto, Kind);
            dto.Freeze();
            string buffer = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.SerializeToJson(dto);
            var recdMsg = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.DeserializeFromJson<TestModels.JsonNewtonSoft.MyDTO>(buffer);
            recdMsg!.Freeze();
            return buffer.Length;
        }
    }
}
