using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Storage;
using DTOMaker.Runtime;
using MemoryPack;
using MessagePack;
using SampleDTO.Strings;
using System;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MemoryPackable]
    public sealed partial class MemoryPackStringsDTO : IStringsDTO
    {
        public void Freeze() { }

        public bool IsFrozen => false;

        public IEntityBase PartCopy()
        {
            return new MemoryPackStringsDTO
            {
                Field05_Value = this.Field05_Value,
                Field05_HasValue = this.Field05_HasValue
            };
        }

        [MemoryPackInclude] public string Field05_Value { get; set; } = "";
        [MemoryPackInclude] public bool Field05_HasValue { get; set; }

        [MemoryPackIgnore]
        public string? Field05
        {
            get
            {
                return Field05_HasValue ? Field05_Value : null;
            }
            set
            {
                if (value is null)
                {
                    Field05_HasValue = false;
                    Field05_Value = "";
                }
                else
                {
                    Field05_HasValue = true;
                    Field05_Value = value;
                }
            }
        }
    }

    // todo DTORoundtripOctets
    //[SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripStrings
    {
        [Params(ValueKind.StringNull, ValueKind.StringFull)]
        public ValueKind Kind;

        private readonly IDataStore DataStore = new DataFac.Storage.Testing.TestDataStore();

        private static readonly string StringWith255Chars =
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcde";

        private SampleDTO.Strings.MessagePack.StringsDTO MakeMyDTO_MessagePack(ValueKind kind)
        {
            var dto = new SampleDTO.Strings.MessagePack.StringsDTO();
            switch (Kind)
            {
                case ValueKind.StringNull:
                    dto.Field05 = null;
                    break;
                case ValueKind.StringZero:
                    dto.Field05 = string.Empty;
                    break;
                case ValueKind.StringFull:
                    dto.Field05 = StringWith255Chars;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
            return dto;
        }

        [Benchmark(Baseline = true)]
        public int Roundtrip_MessagePack()
        {
            var dto = MakeMyDTO_MessagePack(Kind);
            dto.Freeze();
            var buffer = MessagePackSerializer.Serialize<SampleDTO.Strings.MessagePack.StringsDTO>(dto);
            var copy = MessagePackSerializer.Deserialize<SampleDTO.Strings.MessagePack.StringsDTO>(buffer, out int bytesRead);
            dto.Freeze();
            return 0;
        }

        private SampleDTO.Strings.MemBlocks.StringsDTO MakeMyDTO_MemBlocks(ValueKind kind)
        {
            var dto = new SampleDTO.Strings.MemBlocks.StringsDTO();
            switch (Kind)
            {
                case ValueKind.StringNull:
                    dto.Field05 = null;
                    break;
                case ValueKind.StringZero:
                    dto.Field05 = string.Empty;
                    break;
                case ValueKind.StringFull:
                    dto.Field05 = StringWith255Chars;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
            return dto;
        }

        [Benchmark]
        public async ValueTask<int> Roundtrip_MemBlocks()
        {
            var dto = MakeMyDTO_MemBlocks(Kind);
            await dto.Pack(DataStore);
            var buffers = dto.GetBuffers();
            var copy = SampleDTO.Strings.MemBlocks.StringsDTO.CreateFrom(buffers);
            return 0;
        }

        private MemoryPackStringsDTO MakeStringsDTO_MemoryPack(ValueKind id)
        {
            var dto = new MemoryPackStringsDTO();
            switch (Kind)
            {
                case ValueKind.StringNull:
                    dto.Field05 = null;
                    break;
                case ValueKind.StringZero:
                    dto.Field05 = string.Empty;
                    break;
                case ValueKind.StringFull:
                    dto.Field05 = StringWith255Chars;
                    break;
                default:
                    break;
            }
            return dto;
        }

        [Benchmark]
        public int Roundtrip_MemoryPack()
        {
            var dto = MakeStringsDTO_MemoryPack(Kind);
            dto.Freeze();
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<MemoryPackStringsDTO>(dto);
            var copy = MemoryPackSerializer.Deserialize<MemoryPackStringsDTO>(buffer.Span);
            dto.Freeze();
            return 0;
        }
    }
}