using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Memory;
using DataFac.Storage;
using DTOMaker.Runtime;
using DTOMaker.Runtime.JsonSystemText;
using MemoryPack;
using SampleDTO.Basic;
using System;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MemoryPackable]
    public sealed partial class MemoryPackMyDTO : IMyDTO
    {
        public void Freeze() { }

        public bool IsFrozen => false;

        public IEntityBase PartCopy()
        {
            return new MemoryPackMyDTO
            {
                Field01 = this.Field01,
                Field02LE = this.Field02LE,
                Field03BE = this.Field03BE,
                Field04 = this.Field04,
                Field05 = this.Field05,
                Field06 = this.Field06,
                Field07 = this.Field07,
                Field08 = this.Field08,
                Field09 = this.Field09
            };
        }

        [MemoryPackInclude] public bool Field01 { get; set; }
        [MemoryPackInclude] public double Field02LE { get; set; }
        [MemoryPackInclude] public double Field03BE { get; set; }
        [MemoryPackInclude] public Guid Field04 { get; set; }
        [MemoryPackInclude] public string? Field05 { get; set; }
        [MemoryPackInclude] public ReadOnlyMemory<byte> Field06 { get; set; }
        [MemoryPackInclude] public PairOfInt16 Field07 { get; set; }
        [MemoryPackInclude] public PairOfInt32 Field08 { get; set; }
        [MemoryPackInclude] public PairOfInt64 Field09 { get; set; }
        [MemoryPackIgnore] Octets? IMyDTO.Field06 { get => Octets.UnsafeWrap(this.Field06); set => this.Field06 = (value is null) ? default : value.AsMemory(); }
    }

    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripBasics
    {
        [Params(ValueKind.DoubleLE, ValueKind.PairOfInt16, ValueKind.PairOfInt32, ValueKind.PairOfInt64)]
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

        private SampleDTO.Basic.JsonSystemText.MyDTO MakeMyDTO_JsonSystemText(ValueKind id)
        {
            var dto = new SampleDTO.Basic.JsonSystemText.MyDTO();
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
            return dto;
        }

        private MemoryPackMyDTO MakeMyDTO_MemoryPack(ValueKind id)
        {
            var dto = new MemoryPackMyDTO();
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
            return dto;
        }

        [Benchmark(Baseline = true)]
        public int Roundtrip_MemoryPack()
        {
            var dto = MakeMyDTO_MemoryPack(Kind);
            dto.Freeze();
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<MemoryPackMyDTO>(dto);
            var copy = MemoryPackSerializer.Deserialize<MemoryPackMyDTO>(buffer.Span);
            dto.Freeze();
            return buffer.Length;
        }

        [Benchmark]
        public int Roundtrip_JsonSystemText()
        {
            var dto = MakeMyDTO_JsonSystemText(Kind);
            dto.Freeze();
            string buffer = dto.SerializeToJson();
            var recdMsg = buffer.DeserializeFromJson<SampleDTO.Basic.JsonSystemText.MyDTO>();
            recdMsg!.Freeze();
            return buffer.Length;
        }
    }
}
