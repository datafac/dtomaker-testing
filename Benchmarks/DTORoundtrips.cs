using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Storage;
using MemoryPack;
using MessagePack;
using SampleDTO.Basic;
using SampleDTO.Basic.MemoryPack;
using SampleDTO.Basic.NetStrux;
using System;
using System.Threading.Tasks;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripBasics
    {
        [Params(ValueKind.Bool, ValueKind.Guid, ValueKind.DoubleLE)]
        public ValueKind Kind;

        private readonly IDataStore DataStore = new DataFac.Storage.Testing.TestDataStore();

        private static readonly Guid guidValue = new("cc8af561-5172-43e6-8090-5dc1b2d02e07");

        private static readonly string StringWith128Chars =
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef" +
            "0123456789abcdef";

        private SampleDTO.Basic.MessagePack.MyDTO MakeMyDTO_MessagePack(ValueKind id)
        {
            var dto = new SampleDTO.Basic.MessagePack.MyDTO();
            switch (Kind)
            {
                case ValueKind.Bool:
                    dto.Field01 = true;
                    break;
                case ValueKind.DoubleLE:
                    dto.Field02LE = Double.MaxValue;
                    break;
                case ValueKind.Guid:
                    dto.Field03 = guidValue;
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
                    dto.Field03 = guidValue;
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

        private SampleDTO.Basic.MemBlocks.MyDTO MakeMyDTO_MemBlocks(ValueKind id)
        {
            var dto = new SampleDTO.Basic.MemBlocks.MyDTO();
            switch (Kind)
            {
                case ValueKind.Bool:
                    dto.Field01 = true;
                    break;
                case ValueKind.DoubleLE:
                    dto.Field02LE = Double.MaxValue;
                    break;
                case ValueKind.Guid:
                    dto.Field03 = guidValue;
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

        private NetStruxMyDTO MakeMyDTO_NetStrux(ValueKind id)
        {
            var dto = new NetStruxMyDTO();
            switch (Kind)
            {
                case ValueKind.Bool:
                    dto.Field01 = true;
                    break;
                case ValueKind.DoubleLE:
                    dto.Field02LE = Double.MaxValue;
                    break;
                case ValueKind.Guid:
                    dto.Field03 = guidValue;
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
        public int Roundtrip_MessagePack()
        {
            var dto = MakeMyDTO_MessagePack(Kind);
            dto.Freeze();
            ReadOnlyMemory<byte> buffer = MessagePackSerializer.Serialize<SampleDTO.Basic.MessagePack.MyDTO>(dto);
            var copy = MessagePackSerializer.Deserialize<SampleDTO.Basic.MessagePack.MyDTO>(buffer, out int bytesRead);
            dto.Freeze();
            return buffer.Length;
        }

        [Benchmark]
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
        public async ValueTask<long> Roundtrip_MemBlocks()
        {
            var dto = MakeMyDTO_MemBlocks(Kind);
            await dto.Pack(DataStore);
            var buffers = dto.GetBuffers();
            var copy = SampleDTO.Basic.MemBlocks.MyDTO.CreateFrom(buffers);
            return buffers.Length;
        }

        [Benchmark]
        public int Roundtrip_NetStrux()
        {
            var dto = MakeMyDTO_NetStrux(Kind);
            dto.Freeze();
            Span<byte> buffer = stackalloc byte[256];
            dto.TryWrite(buffer);
            var copy = new NetStruxMyDTO();
            copy.TryRead(buffer);
            return buffer.Length;
        }
    }
}
