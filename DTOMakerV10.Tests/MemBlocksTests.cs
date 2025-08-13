using DataFac.Memory;
using DataFac.Storage;
using DTOMaker.Runtime;
using DTOMaker.Runtime.MemBlocks;
using DTOMakerV10.Models.MemBlocks;
using Shouldly;
using System;
using System.Buffers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class MemBlocksTests
    {

        private async Task RoundtripAsync<TValue, TMsg>(
            IDataStore dataStore, TValue value, string expectedHeadBytes, string expectedDataBytes,
            Action<TMsg, TValue> setValueFunc, Func<ReadOnlySequence<byte>, TMsg> createFunc, Func<TMsg, TValue> getValueFunc)
            where TMsg : EntityBase, IEntityBase, new()
        {
            var sendMsg = new TMsg();
            setValueFunc(sendMsg, value);
            await sendMsg.Pack(dataStore);

            // act
            var entityId = sendMsg.GetEntityId();
            var buffers = sendMsg.GetBuffers();

            TMsg recdMsg = createFunc(buffers);
            await recdMsg.UnpackAll(dataStore);

            // assert
            // - value
            TValue copyValue = getValueFunc(recdMsg);
            copyValue.ShouldBe(value);

            // - wire data
            var headBuffer = buffers.Slice(0, 16).Compact();
            string.Join("-", headBuffer.ToArray().Select(b => b.ToString("X2"))).ShouldStartWith(expectedHeadBytes);
            var dataBuffer = buffers.Slice(16).Compact();
            string.Join("-", dataBuffer.ToArray().Select(b => b.ToString("X2"))).ShouldStartWith(expectedDataBytes);

            // - equality
            recdMsg.ShouldNotBeNull();
            recdMsg.ShouldBe(sendMsg);
            recdMsg!.Equals(sendMsg).ShouldBeTrue();
            recdMsg.GetHashCode().ShouldBe(sendMsg.GetHashCode());
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-09-00-00-00-11-00-00-00-00-00-00-00", "00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-09-00-00-00-11-00-00-00-00-00-00-00", "01")]
        public async Task Roundtrip_Boolean(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Boolean value = kind switch
            {
                ValueKind.DefVal => false,
                ValueKind.PosOne => true,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Boolean, Data_Boolean>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Boolean(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-01-00-00-00-11-00-00-00-00-00-00-00", "00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-01-00-00-00-11-00-00-00-00-00-00-00", "01")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-01-00-00-00-11-00-00-00-00-00-00-00", "FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-01-00-00-00-11-00-00-00-00-00-00-00", "7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-01-00-00-00-11-00-00-00-00-00-00-00", "80")]
        public async Task Roundtrip_SByte(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            SByte value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => SByte.MaxValue,
                ValueKind.MinVal => SByte.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<SByte, Data_SByte>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_SByte(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-02-00-00-00-11-00-00-00-00-00-00-00", "00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-02-00-00-00-11-00-00-00-00-00-00-00", "01")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-02-00-00-00-11-00-00-00-00-00-00-00", "FF")]
        public async Task Roundtrip_Byte(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Byte value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => Byte.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Byte, Data_Byte>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Byte(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-03-00-00-00-21-00-00-00-00-00-00-00", "00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-03-00-00-00-21-00-00-00-00-00-00-00", "01-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-03-00-00-00-21-00-00-00-00-00-00-00", "FF-FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-03-00-00-00-21-00-00-00-00-00-00-00", "FF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-03-00-00-00-21-00-00-00-00-00-00-00", "00-80")]
        public async Task Roundtrip_Int16(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Int16 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Int16.MaxValue,
                ValueKind.MinVal => Int16.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Int16, Data_Int16>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Int16(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-04-00-00-00-21-00-00-00-00-00-00-00", "00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-04-00-00-00-21-00-00-00-00-00-00-00", "01-00")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-04-00-00-00-21-00-00-00-00-00-00-00", "FF-FF")]
        public async Task Roundtrip_UInt16(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            UInt16 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt16.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<UInt16, Data_UInt16>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_UInt16(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-05-00-00-00-31-00-00-00-00-00-00-00", "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-05-00-00-00-31-00-00-00-00-00-00-00", "01-00-00-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-05-00-00-00-31-00-00-00-00-00-00-00", "FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-05-00-00-00-31-00-00-00-00-00-00-00", "FF-FF-FF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-05-00-00-00-31-00-00-00-00-00-00-00", "00-00-00-80")]
        public async Task Roundtrip_Int32(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Int32 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Int32.MaxValue,
                ValueKind.MinVal => Int32.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Int32, Data_Int32>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Int32(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-06-00-00-00-31-00-00-00-00-00-00-00", "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-06-00-00-00-31-00-00-00-00-00-00-00", "01-00-00-00")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-06-00-00-00-31-00-00-00-00-00-00-00", "FF-FF-FF-FF")]
        public async Task Roundtrip_UInt32(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            UInt32 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt32.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<UInt32, Data_UInt32>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_UInt32(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-07-00-00-00-41-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-07-00-00-00-41-00-00-00-00-00-00-00", "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-07-00-00-00-41-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-07-00-00-00-41-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-FF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-07-00-00-00-41-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-80")]
        public async Task Roundtrip_Int64(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Int64 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Int64.MaxValue,
                ValueKind.MinVal => Int64.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Int64, Data_Int64>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Int64(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-08-00-00-00-41-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-08-00-00-00-41-00-00-00-00-00-00-00", "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-08-00-00-00-41-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-FF-FF")]
        public async Task Roundtrip_UInt64(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            UInt64 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt64.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<UInt64, Data_UInt64>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_UInt64(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-0A-00-00-00-21-00-00-00-00-00-00-00", "00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-0A-00-00-00-21-00-00-00-00-00-00-00", "20-00")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-0A-00-00-00-21-00-00-00-00-00-00-00", "FF-FF")]
        public async Task Roundtrip_Char(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Char value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => ' ',
                ValueKind.MaxVal => Char.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Char, Data_Char>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Char(b), (m) => m.Value);
        }

#if NET8_0_OR_GREATER
        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-15-00-00-00-21-00-00-00-00-00-00-00", "00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-15-00-00-00-21-00-00-00-00-00-00-00", "00-3C")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-15-00-00-00-21-00-00-00-00-00-00-00", "00-BC")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-15-00-00-00-21-00-00-00-00-00-00-00", "FF-7B")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-15-00-00-00-21-00-00-00-00-00-00-00", "FF-FB")]
        [InlineData(ValueKind.MinInc, "7C-5F-01-01-15-00-00-00-21-00-00-00-00-00-00-00", "01-00")]
        [InlineData(ValueKind.NegInf, "7C-5F-01-01-15-00-00-00-21-00-00-00-00-00-00-00", "00-FC")]
        [InlineData(ValueKind.PosInf, "7C-5F-01-01-15-00-00-00-21-00-00-00-00-00-00-00", "00-7C")]
        [InlineData(ValueKind.NotNum, "7C-5F-01-01-15-00-00-00-21-00-00-00-00-00-00-00", "00-FE")]
        public async Task Roundtrip_Half(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Half value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => Half.One,
                ValueKind.NegOne => Half.NegativeOne,
                ValueKind.MaxVal => Half.MaxValue,
                ValueKind.MinVal => Half.MinValue,
                ValueKind.MinInc => Half.Epsilon,
                ValueKind.NegInf => Half.NegativeInfinity,
                ValueKind.PosInf => Half.PositiveInfinity,
                ValueKind.NotNum => Half.NaN,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Half, Data_Half>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Half(b), (m) => m.Value);
        }
#endif

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-0B-00-00-00-31-00-00-00-00-00-00-00", "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-0B-00-00-00-31-00-00-00-00-00-00-00", "00-00-80-3F")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-0B-00-00-00-31-00-00-00-00-00-00-00", "00-00-80-BF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-0B-00-00-00-31-00-00-00-00-00-00-00", "FF-FF-7F-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-0B-00-00-00-31-00-00-00-00-00-00-00", "FF-FF-7F-FF")]
        [InlineData(ValueKind.MinInc, "7C-5F-01-01-0B-00-00-00-31-00-00-00-00-00-00-00", "01-00-00-00")]
        [InlineData(ValueKind.NegInf, "7C-5F-01-01-0B-00-00-00-31-00-00-00-00-00-00-00", "00-00-80-FF")]
        [InlineData(ValueKind.PosInf, "7C-5F-01-01-0B-00-00-00-31-00-00-00-00-00-00-00", "00-00-80-7F")]
        [InlineData(ValueKind.NotNum, "7C-5F-01-01-0B-00-00-00-31-00-00-00-00-00-00-00", "00-00-C0-FF")]
        public async Task Roundtrip_Single(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Single value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1F,
                ValueKind.NegOne => -1F,
                ValueKind.MaxVal => Single.MaxValue,
                ValueKind.MinVal => Single.MinValue,
                ValueKind.MinInc => Single.Epsilon,
                ValueKind.NegInf => Single.NegativeInfinity,
                ValueKind.PosInf => Single.PositiveInfinity,
                ValueKind.NotNum => Single.NaN,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Single, Data_Single>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Single(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-0C-00-00-00-41-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-0C-00-00-00-41-00-00-00-00-00-00-00", "00-00-00-00-00-00-F0-3F")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-0C-00-00-00-41-00-00-00-00-00-00-00", "00-00-00-00-00-00-F0-BF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-0C-00-00-00-41-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-EF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-0C-00-00-00-41-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-EF-FF")]
        [InlineData(ValueKind.MinInc, "7C-5F-01-01-0C-00-00-00-41-00-00-00-00-00-00-00", "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegInf, "7C-5F-01-01-0C-00-00-00-41-00-00-00-00-00-00-00", "00-00-00-00-00-00-F0-FF")]
        [InlineData(ValueKind.PosInf, "7C-5F-01-01-0C-00-00-00-41-00-00-00-00-00-00-00", "00-00-00-00-00-00-F0-7F")]
        [InlineData(ValueKind.NotNum, "7C-5F-01-01-0C-00-00-00-41-00-00-00-00-00-00-00", "00-00-00-00-00-00-F8-FF")]
        public async Task Roundtrip_Double(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Double value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1D,
                ValueKind.NegOne => -1D,
                ValueKind.MaxVal => Double.MaxValue,
                ValueKind.MinVal => Double.MinValue,
                ValueKind.MinInc => Double.Epsilon,
                ValueKind.NegInf => Double.NegativeInfinity,
                ValueKind.PosInf => Double.PositiveInfinity,
                ValueKind.NotNum => Double.NaN,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Double, Data_Double>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Double(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-0D-00-00-00-51-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-0D-00-00-00-51-00-00-00-00-00-00-00", "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-0D-00-00-00-51-00-00-00-00-00-00-00", "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-80")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-0D-00-00-00-51-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-00")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-0D-00-00-00-51-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-80")]
        public async Task Roundtrip_Decimal(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Decimal value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => Decimal.One,
                ValueKind.NegOne => Decimal.MinusOne,
                ValueKind.MaxVal => Decimal.MaxValue,
                ValueKind.MinVal => Decimal.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Decimal, Data_Decimal>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Decimal(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-10-00-00-00-51-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.MinInc, "7C-5F-01-01-10-00-00-00-51-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-10-00-00-00-51-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-10-00-00-00-51-00-00-00-00-00-00-00", "FF-A1-38-CB-70-04-06-4E-9D-88-34-61-EB-52-57-EB")]
        public async Task Roundtrip_Guid(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Guid value = kind switch
            {
                ValueKind.DefVal => Guid.Empty,
                ValueKind.MinInc => new Guid("00000000-0000-0000-0000-000000000001"),
                ValueKind.MaxVal => new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                ValueKind.PosOne => new Guid("cb38a1ff-0470-4e06-9d88-3461eb5257eb"),
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Guid, Data_Guid>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Guid(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-11-00-00-00-71-00-00-00-00-00-00-00", "55-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-11-00-00-00-71-00-00-00-00-00-00-00", "55-06-61-62-63-64-65-66-00-00-00-00-00-00-00-00")]
        public async Task Roundtrip_String(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            string value = kind switch
            {
                //ValueKind.DefVal => null,
                ValueKind.MinVal => string.Empty,
                ValueKind.PosOne => "abcdef",
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<string, Data_String>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_String(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-12-00-00-00-71-00-00-00-00-00-00-00", "55-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-12-00-00-00-71-00-00-00-00-00-00-00", "55-06-61-62-63-64-65-66-00-00-00-00-00-00-00-00")]
        public async Task Roundtrip_Octets(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Octets value = kind switch
            {
                //ValueKind.DefVal => null,
                ValueKind.MinVal => Octets.Empty,
                ValueKind.PosOne => new Octets(Encoding.UTF8.GetBytes("abcdef")),
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Octets, Data_Octets>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Octets(b), (m) => m.Value);
        }

#if NET8_0_OR_GREATER
        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-0E-00-00-00-51-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-0E-00-00-00-51-00-00-00-00-00-00-00", "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-0E-00-00-00-51-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-0E-00-00-00-51-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-0E-00-00-00-51-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-80")]
        public async Task Roundtrip_Int128(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            Int128 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Int128.MaxValue,
                ValueKind.MinVal => Int128.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<Int128, Data_Int128>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_Int128(b), (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-0F-00-00-00-51-00-00-00-00-00-00-00", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-0F-00-00-00-51-00-00-00-00-00-00-00", "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-0F-00-00-00-51-00-00-00-00-00-00-00", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF")]
        public async Task Roundtrip_UInt128(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            UInt128 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt128.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<UInt128, Data_UInt128>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_UInt128(b), (m) => m.Value);
        }

#endif

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-01-16-00-00-00-31-00-00-00-00-00-00-00", "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-01-16-00-00-00-31-00-00-00-00-00-00-00", "01-00-01-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-01-16-00-00-00-31-00-00-00-00-00-00-00", "FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-01-16-00-00-00-31-00-00-00-00-00-00-00", "FF-7F-FF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-01-16-00-00-00-31-00-00-00-00-00-00-00", "00-80-00-80")]

        public async Task Roundtrip_PairOfInt16Async(ValueKind kind, string expectedHeadBytes, string expectedDataBytes)
        {
            PairOfInt16 value = kind switch
            {
                ValueKind.DefVal => new PairOfInt16(default, default),
                ValueKind.PosOne => new PairOfInt16(1, 1),
                ValueKind.NegOne => new PairOfInt16(-1, -1),
                ValueKind.MaxVal => new PairOfInt16(Int16.MaxValue, Int16.MaxValue),
                ValueKind.MinVal => new PairOfInt16(Int16.MinValue, Int16.MinValue),
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            await RoundtripAsync<PairOfInt16, Data_PairOfInt16>(dataStore, value, expectedHeadBytes, expectedDataBytes, (m, v) => { m.Value = v; }, (b) => new Data_PairOfInt16(b), (m) => m.Value);
        }

    }
}
