using DataFac.Storage;
using DTOMaker.Runtime;
using DTOMaker.Runtime.MemBlocks;
using DTOMakerV10.Models;
using DTOMakerV10.Models.MemBlocks;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class MemBlocksTests
    {

        private async Task RoundtripAsync<TValue, TMsg>(
            IDataStore dataStore, TValue value, string expectedHeadBytes, string expectedDataBytes,
            Action<TMsg, TValue> setValueFunc, Func<ReadOnlyMemory<byte>, TMsg> createFunc, Func<TMsg, TValue> getValueFunc)
            where TMsg : EntityBase, IFreezable, new()
        {
            var sendMsg = new TMsg();
            setValueFunc(sendMsg, value);
            await sendMsg.Pack(dataStore);

            // act
            var entityId = sendMsg.GetEntityId();
            var buffer = sendMsg.GetBuffer();

            TMsg recdMsg = createFunc(buffer);
            await recdMsg.UnpackAll(dataStore);

            // assert
            // - value
            TValue copyValue = getValueFunc(recdMsg);
            copyValue.ShouldBe(value);

            // - wire data
            var headBuffer = buffer.Slice(0, 64);
            string.Join("-", headBuffer.ToArray().Select(b => b.ToString("X2"))).ShouldStartWith(expectedHeadBytes);
            var dataBuffer = buffer.Slice(64);
            string.Join("-", dataBuffer.ToArray().Select(b => b.ToString("X2"))).ShouldStartWith(expectedDataBytes);

            // - equality
            recdMsg.ShouldNotBeNull();
            recdMsg.ShouldBe(sendMsg);
            recdMsg!.Equals(sendMsg).ShouldBeTrue();
            recdMsg.GetHashCode().ShouldBe(sendMsg.GetHashCode());
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "01")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "01")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "80")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "01")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-01-00-00-00-00-00-00-00-", "FF")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "01-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "FF-FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "FF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-80")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "01-00")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "FF-FF")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "01-00-00-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "FF-FF-FF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "00-00-00-80")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "01-00-00-00")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "FF-FF-FF-FF")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "FF-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "FF-FF-FF-FF-FF-FF-FF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "00-00-00-00-00-00-00-80")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "FF-FF-FF-FF-FF-FF-FF-FF")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "20-00")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "FF-FF")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-3C")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-BC")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "FF-7B")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "FF-FB")]
        [InlineData(ValueKind.MinInc, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "01-00")]
        [InlineData(ValueKind.NegInf, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-FC")]
        [InlineData(ValueKind.PosInf, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-7C")]
        [InlineData(ValueKind.NotNum, "7C-5F-01-00-00-00-00-00-11-00-00-00-00-00-00-00-", "00-FE")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "00-00-80-3F")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "00-00-80-BF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "FF-FF-7F-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "FF-FF-7F-FF")]
        [InlineData(ValueKind.MinInc, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "01-00-00-00")]
        [InlineData(ValueKind.NegInf, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "00-00-80-FF")]
        [InlineData(ValueKind.PosInf, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "00-00-80-7F")]
        [InlineData(ValueKind.NotNum, "7C-5F-01-00-00-00-00-00-21-00-00-00-00-00-00-00-", "00-00-C0-FF")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "00-00-00-00-00-00-F0-3F")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "00-00-00-00-00-00-F0-BF")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "FF-FF-FF-FF-FF-FF-EF-7F")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "FF-FF-FF-FF-FF-FF-EF-FF")]
        [InlineData(ValueKind.MinInc, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegInf, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "00-00-00-00-00-00-F0-FF")]
        [InlineData(ValueKind.PosInf, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "00-00-00-00-00-00-F0-7F")]
        [InlineData(ValueKind.NotNum, "7C-5F-01-00-00-00-00-00-31-00-00-00-00-00-00-00-", "00-00-00-00-00-00-F8-FF")]
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
        [InlineData(ValueKind.DefVal, "7C-5F-01-00-00-00-00-00-41-00-00-00-00-00-00-00-", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "7C-5F-01-00-00-00-00-00-41-00-00-00-00-00-00-00-", "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegOne, "7C-5F-01-00-00-00-00-00-41-00-00-00-00-00-00-00-", "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-80")]
        [InlineData(ValueKind.MaxVal, "7C-5F-01-00-00-00-00-00-41-00-00-00-00-00-00-00-", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-00")]
        [InlineData(ValueKind.MinVal, "7C-5F-01-00-00-00-00-00-41-00-00-00-00-00-00-00-", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-80")]
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

        // todo Guid, half, int128, uint128

    }
}
