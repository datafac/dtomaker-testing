using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Memory;
using DataFac.Storage;
using DTOMaker.Runtime;
using DTOMaker.Runtime.MessagePack;
using MemoryPack;
using MessagePack;
using Newtonsoft.Json;
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
                Field02BE = this.Field02BE,
                Field03 = this.Field03,
                Field05_Length = this.Field05_Length,
                Field05_Data = this.Field05_Data,
                Field05 = this.Field05
            };
        }

        [MemoryPackInclude] public bool Field01 { get; set; }
        [MemoryPackInclude] public double Field02LE { get; set; }
        [MemoryPackInclude] public double Field02BE { get; set; }
        [MemoryPackInclude] public Guid Field03 { get; set; }
        [MemoryPackIgnore] public short Field05_Length { get; set; }
        [MemoryPackIgnore] public ReadOnlyMemory<byte> Field05_Data { get; set; }
        [MemoryPackInclude] public string? Field05 { get; set; }
        [MemoryPackInclude] public PairOfInt16 Field07 { get; set; }
    }

    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripBasics
    {
        [Params(ValueKind.Bool, ValueKind.Guidqqq, ValueKind.DoubleLE, ValueKind.PairOfInt16)]
        public ValueKind Kind;

        private readonly IDataStore DataStore = new DataFac.Storage.Testing.TestDataStore();

        private static readonly Guid guidValue = new("cc8af561-5172-43e6-8090-5dc1b2d02e07");

        private static readonly PairOfInt16 pairOfInt16Value = new PairOfInt16((Int16)1, (Int16)(-1));

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
                case ValueKind.Guidqqq:
                    dto.Field03 = guidValue;
                    break;
                case ValueKind.PairOfInt16:
                    dto.Field07 = pairOfInt16Value;
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

        private SampleDTO.Basic.JsonNewtonSoft.MyDTO MakeMyDTO_JsonNewtonSoft(ValueKind id)
        {
            var dto = new SampleDTO.Basic.JsonNewtonSoft.MyDTO();
            switch (Kind)
            {
                case ValueKind.Bool:
                    dto.Field01 = true;
                    break;
                case ValueKind.DoubleLE:
                    dto.Field02LE = Double.MaxValue;
                    break;
                case ValueKind.Guidqqq:
                    dto.Field03 = guidValue;
                    break;
                case ValueKind.PairOfInt16:
                    dto.Field07 = pairOfInt16Value;
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
                case ValueKind.Guidqqq:
                    dto.Field03 = guidValue;
                    break;
                case ValueKind.PairOfInt16:
                    dto.Field07 = pairOfInt16Value;
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
                case ValueKind.Guidqqq:
                    dto.Field03 = guidValue;
                    break;
                case ValueKind.PairOfInt16:
                    dto.Field07 = pairOfInt16Value;
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
            var buffer = dto.SerializeToMessagePack<SampleDTO.Basic.MessagePack.MyDTO>();
            var copy = buffer.DeserializeFromMessagePack<SampleDTO.Basic.MessagePack.MyDTO>();
            copy.Freeze();
            return buffer.Length;
        }

        private static readonly JsonSerializerSettings jsonNSSettings = new JsonSerializerSettings()
        {
            //Formatting = Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
        };

        [Benchmark]
        public int Roundtrip_JsonNewtonSoft()
        {
            var dto = MakeMyDTO_JsonNewtonSoft(Kind);
            dto.Freeze();
            string buffer = JsonConvert.SerializeObject(dto, jsonNSSettings);
            SampleDTO.Basic.JsonNewtonSoft.MyDTO? recdMsg = JsonConvert.DeserializeObject<SampleDTO.Basic.JsonNewtonSoft.MyDTO>(buffer, jsonNSSettings);
            recdMsg!.Freeze();
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

    }
}
