﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Memory;
using DataFac.Storage;
using MemoryPack;
using MessagePack;
using SampleDTO.Binary;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Benchmarks
{
    //[SimpleJob(RuntimeMoniker.Net80)]
    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripBinary
    {
        [Params(ValueKind.BinaryNull, ValueKind.BinaryZero, ValueKind.BinarySmall, ValueKind.BinaryLarge)]
        public ValueKind Kind;

        private static readonly IDataStore DataStore = new DataFac.Storage.Testing.TestDataStore();

        private static readonly Octets SmallBinaryValue = new Octets(Enumerable.Range(0, 60).Select(i => (byte)i).ToArray());
        private static readonly Octets LargeBinaryValue = new Octets(Enumerable.Range(0, 256).Select(i => (byte)i).ToArray());

        private SampleDTO.Binary.MessagePack.BinaryDTO MakeMyDTO_MessagePack(ValueKind kind)
        {
            var dto = new SampleDTO.Binary.MessagePack.BinaryDTO();
            IBinaryDTO intf = dto;
            switch (Kind)
            {
                case ValueKind.BinaryNull:
                    intf.Value = null;
                    break;
                case ValueKind.BinaryZero:
                    intf.Value = Octets.Empty;
                    break;
                case ValueKind.BinarySmall:
                    intf.Value = SmallBinaryValue;
                    break;
                case ValueKind.BinaryLarge:
                    intf.Value = LargeBinaryValue;
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
            var buffer = MessagePackSerializer.Serialize<SampleDTO.Binary.MessagePack.BinaryDTO>(dto);
            var copy = MessagePackSerializer.Deserialize<SampleDTO.Binary.MessagePack.BinaryDTO>(buffer, out int bytesRead);
            dto.Freeze();
            return 0;
        }

        private SampleDTO.Binary.MemBlocks.BinaryDTO MakeMyDTO_MemBlocks(ValueKind kind)
        {
            var dto = new SampleDTO.Binary.MemBlocks.BinaryDTO();
            IBinaryDTO intf = dto;
            switch (Kind)
            {
                case ValueKind.BinaryNull:
                    intf.Value = null;
                    break;
                case ValueKind.BinaryZero:
                    intf.Value = Octets.Empty;
                    break;
                case ValueKind.BinarySmall:
                    intf.Value = SmallBinaryValue;
                    break;
                case ValueKind.BinaryLarge:
                    intf.Value = LargeBinaryValue;
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
            var buffer = dto.GetBuffer();
            var copy = SampleDTO.Binary.MemBlocks.BinaryDTO.CreateFrom(buffer);
            return 0;
        }
    }
}