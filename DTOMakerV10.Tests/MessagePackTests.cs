using DTOMaker.Runtime;
using FluentAssertions;
using MessagePack;
using System;
using System.Linq;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class MessagePackTests
    {
        private void Roundtrip2<TValue, TMsg>(TValue value, string expectedBytes, Action<TMsg, TValue> setValueFunc, Func<TMsg, TValue> getValueFunc)
            where TMsg : IFreezable, IEquatable<TMsg>, new()
        {
            var sendMsg = new TMsg();
            setValueFunc(sendMsg, value);
            sendMsg.Freeze();

            // act
            var buffer = MessagePackSerializer.Serialize<TMsg>(sendMsg);
            TMsg recdMsg = MessagePackSerializer.Deserialize<TMsg>(buffer);
            recdMsg.Freeze();

            // assert
            // - value
            TValue copyValue = getValueFunc(recdMsg);
            copyValue.Should().Be(value);

            // - wire data
            string.Join("-", buffer.Select(b => b.ToString("X2"))).Should().Be(expectedBytes);

            // - equality
            recdMsg.Should().NotBeNull();
            recdMsg!.Equals(sendMsg).Should().BeTrue();
            recdMsg.Should().Be(sendMsg);
            recdMsg.GetHashCode().Should().Be(sendMsg.GetHashCode());
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-C2")]
        [InlineData(ValueKind.PosOne, "92-C0-C3")]
        public void Roundtrip_Boolean(ValueKind kind, string expectedBytes)
        {
            Boolean value = kind switch
            {
                ValueKind.DefVal => false,
                ValueKind.PosOne => true,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Boolean, Models.MessagePack.Data_Boolean>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-00")]
        [InlineData(ValueKind.PosOne, "92-C0-01")]
        [InlineData(ValueKind.NegOne, "92-C0-FF")]
        [InlineData(ValueKind.MaxVal, "92-C0-7F")]
        [InlineData(ValueKind.MinVal, "92-C0-D0-80")]
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

            Roundtrip2<SByte, Models.MessagePack.Data_SByte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-00")]
        [InlineData(ValueKind.PosOne, "92-C0-01")]
        [InlineData(ValueKind.MaxVal, "92-C0-CC-FF")]
        public void Roundtrip_Byte(ValueKind kind, string expectedBytes)
        {
            Byte value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => Byte.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Byte, Models.MessagePack.Data_Byte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-00")]
        [InlineData(ValueKind.PosOne, "92-C0-01")]
        [InlineData(ValueKind.NegOne, "92-C0-FF")]
        [InlineData(ValueKind.MaxVal, "92-C0-CD-7F-FF")]
        [InlineData(ValueKind.MinVal, "92-C0-D1-80-00")]
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

            Roundtrip2<Int16, Models.MessagePack.Data_Int16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-00")]
        [InlineData(ValueKind.PosOne, "92-C0-01")]
        [InlineData(ValueKind.MaxVal, "92-C0-CD-FF-FF")]
        public void Roundtrip_UInt16(ValueKind kind, string expectedBytes)
        {
            UInt16 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt16.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<UInt16, Models.MessagePack.Data_UInt16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-00")]
        [InlineData(ValueKind.PosOne, "92-C0-01")]
        [InlineData(ValueKind.NegOne, "92-C0-FF")]
        [InlineData(ValueKind.MaxVal, "92-C0-CE-7F-FF-FF-FF")]
        [InlineData(ValueKind.MinVal, "92-C0-D2-80-00-00-00")]
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

            Roundtrip2<Int32, Models.MessagePack.Data_Int32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-00")]
        [InlineData(ValueKind.PosOne, "92-C0-01")]
        [InlineData(ValueKind.MaxVal, "92-C0-CE-FF-FF-FF-FF")]
        public void Roundtrip_UInt32(ValueKind kind, string expectedBytes)
        {
            UInt32 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt32.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<UInt32, Models.MessagePack.Data_UInt32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-00")]
        [InlineData(ValueKind.PosOne, "92-C0-01")]
        [InlineData(ValueKind.NegOne, "92-C0-FF")]
        [InlineData(ValueKind.MaxVal, "92-C0-CF-7F-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.MinVal, "92-C0-D3-80-00-00-00-00-00-00-00")]
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

            Roundtrip2<Int64, Models.MessagePack.Data_Int64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-00")]
        [InlineData(ValueKind.PosOne, "92-C0-01")]
        [InlineData(ValueKind.MaxVal, "92-C0-CF-FF-FF-FF-FF-FF-FF-FF-FF")]
        public void Roundtrip_UInt64(ValueKind kind, string expectedBytes)
        {
            UInt64 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt64.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<UInt64, Models.MessagePack.Data_UInt64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-00")]
        [InlineData(ValueKind.PosOne, "92-C0-20")]
        [InlineData(ValueKind.MaxVal, "92-C0-CD-FF-FF")]
        public void Roundtrip_Char(ValueKind kind, string expectedBytes)
        {
            Char value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => ' ',
                ValueKind.MaxVal => Char.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Char, Models.MessagePack.Data_Char>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

#if NET8_0_OR_GREATER
        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-CA-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "92-C0-CA-3F-80-00-00")]
        [InlineData(ValueKind.NegOne, "92-C0-CA-BF-80-00-00")]
        [InlineData(ValueKind.MaxVal, "92-C0-CA-47-7F-E0-00")]
        [InlineData(ValueKind.MinVal, "92-C0-CA-C7-7F-E0-00")]
        [InlineData(ValueKind.MinInc, "92-C0-CA-33-80-00-00")]
        [InlineData(ValueKind.NegInf, "92-C0-CA-FF-80-00-00")]
        [InlineData(ValueKind.PosInf, "92-C0-CA-7F-80-00-00")]
        //[InlineData(ValueKind.NotNum, "92-C0-CA-FF-C0-00-00")] todo NaN equality check fails
        public void Roundtrip_Half(ValueKind kind, string expectedBytes)
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

            Roundtrip2<Half, Models.MessagePack.Data_Half>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }
#endif

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-CA-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "92-C0-CA-3F-80-00-00")]
        [InlineData(ValueKind.NegOne, "92-C0-CA-BF-80-00-00")]
        [InlineData(ValueKind.MaxVal, "92-C0-CA-7F-7F-FF-FF")]
        [InlineData(ValueKind.MinVal, "92-C0-CA-FF-7F-FF-FF")]
        [InlineData(ValueKind.MinInc, "92-C0-CA-00-00-00-01")]
        [InlineData(ValueKind.NegInf, "92-C0-CA-FF-80-00-00")]
        [InlineData(ValueKind.PosInf, "92-C0-CA-7F-80-00-00")]
        //[InlineData(ValueKind.NotNum, "92-C0-CA-FF-C0-00-00")] todo NaN equality check fails
        public void Roundtrip_Single(ValueKind kind, string expectedBytes)
        {
            Single value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Single.MaxValue,
                ValueKind.MinVal => Single.MinValue,
                ValueKind.MinInc => Single.Epsilon,
                ValueKind.NegInf => Single.NegativeInfinity,
                ValueKind.PosInf => Single.PositiveInfinity,
                ValueKind.NotNum => Single.NaN,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Single, Models.MessagePack.Data_Single>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-CB-00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosOne, "92-C0-CB-3F-F0-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegOne, "92-C0-CB-BF-F0-00-00-00-00-00-00")]
        [InlineData(ValueKind.MaxVal, "92-C0-CB-7F-EF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.MinVal, "92-C0-CB-FF-EF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.MinInc, "92-C0-CB-00-00-00-00-00-00-00-01")]
        [InlineData(ValueKind.NegInf, "92-C0-CB-FF-F0-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosInf, "92-C0-CB-7F-F0-00-00-00-00-00-00")]
        //[InlineData(ValueKind.NotNum, "92-C0-CB-FF-C0-00-00")] todo NaN equality check fails
        public void Roundtrip_Double(ValueKind kind, string expectedBytes)
        {
            Double value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Double.MaxValue,
                ValueKind.MinVal => Double.MinValue,
                ValueKind.MinInc => Double.Epsilon,
                ValueKind.NegInf => Double.NegativeInfinity,
                ValueKind.PosInf => Double.PositiveInfinity,
                ValueKind.NotNum => Double.NaN,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Double, Models.MessagePack.Data_Double>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "92-C0-A1-30")]
        [InlineData(ValueKind.PosOne, "92-C0-A1-31")]
        [InlineData(ValueKind.NegOne, "92-C0-A2-2D-31")]
        [InlineData(ValueKind.MaxVal, "92-C0-BD-37-39-32-32-38-31-36-32-35-31-34-32-36-34-33-33-37-35-39-33-35-34-33-39-35-30-33-33-35")]
        [InlineData(ValueKind.MinVal, "92-C0-BE-2D-37-39-32-32-38-31-36-32-35-31-34-32-36-34-33-33-37-35-39-33-35-34-33-39-35-30-33-33-35")]
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

            Roundtrip2<Decimal, Models.MessagePack.Data_Decimal>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        // todo Guid, half, int128, uint128

    }
}