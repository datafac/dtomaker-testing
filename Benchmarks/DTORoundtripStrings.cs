using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Storage;
using MemoryPack;
using MessagePack;
using System;
using System.Threading.Tasks;

namespace Benchmarks
{
    // todo DTORoundtripOctets
    [SimpleJob(RuntimeMoniker.Net80)]
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

        private SampleDTO.Strings.NetStrux.NetStruxStringsDTO MakeMyDTO_NetStrux(ValueKind kind)
        {
            var dto = new SampleDTO.Strings.NetStrux.NetStruxStringsDTO();
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
        public int Roundtrip_NetStrux()
        {
            var dto = MakeMyDTO_NetStrux(Kind);
            dto.Freeze();
            Span<byte> buffer = stackalloc byte[512];
            dto.TryWrite(buffer);
            var copy = new SampleDTO.Basic.NetStrux.NetStruxMyDTO();
            copy.TryRead(buffer);
            return 0;
        }

        private SampleDTO.Strings.MemoryPack.MemoryPackStringsDTO MakeStringsDTO_MemoryPack(ValueKind id)
        {
            var dto = new SampleDTO.Strings.MemoryPack.MemoryPackStringsDTO();
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
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<SampleDTO.Strings.MemoryPack.MemoryPackStringsDTO>(dto);
            var copy = MemoryPackSerializer.Deserialize<SampleDTO.Strings.MemoryPack.MemoryPackStringsDTO>(buffer.Span);
            dto.Freeze();
            return 0;
        }
    }
}