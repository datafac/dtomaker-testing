using DTOMaker.Runtime;
using DTOMaker.Runtime.MemBlocks;
using DTOMakerV10.Models.Basics;
using DTOMakerV10.Models.Basics.MemBlocks;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class MemBlocksTests
    {
        private void Roundtrip<TValue, TMsg>(TValue value, string expectedBytes, Action<TMsg, TValue> setValueFunc, Func<TMsg, TValue> getValueFunc)
            where TMsg : EntityBase, IFreezable, new()
        {
            var sendMsg = new TMsg();
            setValueFunc(sendMsg, value);
            sendMsg.Freeze();

            // act
            var entityId = sendMsg.GetEntityId();
            var buffers = sendMsg.GetBuffers();
            TMsg recdMsg = new TMsg();
            recdMsg.LoadBuffers(buffers);
            recdMsg.Freeze();

            // assert
            // - value
            TValue copyValue = getValueFunc(recdMsg);
            copyValue.Should().Be(value);

            // - wire data
            buffers.Length.Should().Be(1);
            var buffer = buffers.Span[0];
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).Should().Be(expectedBytes);

            // - equality
            recdMsg.Should().NotBeNull();
            recdMsg.Should().Be(sendMsg);
            recdMsg!.Equals(sendMsg).Should().BeTrue();
            recdMsg.GetHashCode().Should().Be(sendMsg.GetHashCode());
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00")]
        [InlineData(ValueKind.PosOne, "01")]
        public void Roundtrip_Boolean(ValueKind kind, string expectedBytes)
        {
            Boolean value = kind switch
            {
                ValueKind.DefVal => false,
                ValueKind.PosOne => true,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<Boolean, Data_Boolean>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00")]
        [InlineData(ValueKind.PosOne, "01")]
        [InlineData(ValueKind.NegOne, "FF")]
        [InlineData(ValueKind.MaxVal, "7F")]
        [InlineData(ValueKind.MinVal, "80")]
        public void Roundtrip_SByte(ValueKind kind, string expectedBytes)
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

            Roundtrip<SByte, Data_SByte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00")]
        [InlineData(ValueKind.PosOne, "01")]
        [InlineData(ValueKind.MaxVal, "FF")]
        public void Roundtrip_Byte(ValueKind kind, string expectedBytes)
        {
            Byte value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => Byte.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<Byte, Data_Byte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00")]
        [InlineData(ValueKind.PosOne, "01-00")]
        [InlineData(ValueKind.NegOne, "FF-FF")]
        [InlineData(ValueKind.MaxVal, "FF-7F")]
        [InlineData(ValueKind.MinVal, "00-80")]
        public void Roundtrip_Int16(ValueKind kind, string expectedBytes)
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

            Roundtrip<Int16, Data_Int16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00")]
        [InlineData(ValueKind.PosOne, "01-00")]
        [InlineData(ValueKind.MaxVal, "FF-FF")]
        public void Roundtrip_UInt16(ValueKind kind, string expectedBytes)
        {
            UInt16 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt16.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<UInt16, Data_UInt16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "01-00-00-00")]
        [InlineData(ValueKind.NegOne, "FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxVal, "FF-FF-FF-7F")]
        [InlineData(ValueKind.MinVal, "00-00-00-80")]
        public void Roundtrip_Int32(ValueKind kind, string expectedBytes)
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

            Roundtrip<Int32, Data_Int32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "01-00-00-00")]
        [InlineData(ValueKind.MaxVal, "FF-FF-FF-FF")]
        public void Roundtrip_UInt32(ValueKind kind, string expectedBytes)
        {
            UInt32 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt32.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<UInt32, Data_UInt32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegOne, "FF-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxVal, "FF-FF-FF-FF-FF-FF-FF-7F")]
        [InlineData(ValueKind.MinVal, "00-00-00-00-00-00-00-80")]
        public void Roundtrip_Int64(ValueKind kind, string expectedBytes)
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

            Roundtrip<Int64, Data_Int64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.MaxVal, "FF-FF-FF-FF-FF-FF-FF-FF")]
        public void Roundtrip_UInt64(ValueKind kind, string expectedBytes)
        {
            UInt64 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt64.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<UInt64, Data_UInt64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00")]
        [InlineData(ValueKind.PosOne, "20-00")]
        [InlineData(ValueKind.MaxVal, "FF-FF")]
        public void Roundtrip_Char(ValueKind kind, string expectedBytes)
        {
            Char value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => ' ',
                ValueKind.MaxVal => Char.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<Char, Data_Char>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00-00-00")]
        [InlineData(ValueKind.PosOne, "00-00-80-3F")]
        [InlineData(ValueKind.NegOne, "00-00-80-BF")]
        [InlineData(ValueKind.MaxVal, "FF-FF-7F-7F")]
        [InlineData(ValueKind.MinVal, "FF-FF-7F-FF")]
        [InlineData(ValueKind.MinInc, "01-00-00-00")]
        [InlineData(ValueKind.NegInf, "00-00-80-FF")]
        [InlineData(ValueKind.PosInf, "00-00-80-7F")]
        [InlineData(ValueKind.NotNum, "00-00-C0-FF")]
        public void Roundtrip_Single(ValueKind kind, string expectedBytes)
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

            Roundtrip<Single, Data_Single>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "00-00-00-00-00-00-F0-3F")]
        [InlineData(ValueKind.NegOne, "00-00-00-00-00-00-F0-BF")]
        [InlineData(ValueKind.MaxVal, "FF-FF-FF-FF-FF-FF-EF-7F")]
        [InlineData(ValueKind.MinVal, "FF-FF-FF-FF-FF-FF-EF-FF")]
        [InlineData(ValueKind.MinInc, "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegInf, "00-00-00-00-00-00-F0-FF")]
        [InlineData(ValueKind.PosInf, "00-00-00-00-00-00-F0-7F")]
        [InlineData(ValueKind.NotNum, "00-00-00-00-00-00-F8-FF")]
        public void Roundtrip_Double(ValueKind kind, string expectedBytes)
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

            Roundtrip<Double, Data_Double>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegOne, "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-80")]
        [InlineData(ValueKind.MaxVal, "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-00")]
        [InlineData(ValueKind.MinVal, "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-80")]
        public void Roundtrip_Decimal(ValueKind kind, string expectedBytes)
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

            Roundtrip<Decimal, Data_Decimal>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        // todo Guid, half, int128, uint128

    }
}
