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
            where TMsg : IFreezable, new()
        {
            var sendMsg = new TMsg();
            setValueFunc(sendMsg, value);
            sendMsg.Freeze();

            // act
            var buffer = MessagePackSerializer.Serialize<TMsg>(sendMsg);
            TMsg recdMsg = MessagePackSerializer.Deserialize<TMsg>(buffer);

            // assert
            // - value
            TValue copyValue = getValueFunc(recdMsg);
            copyValue.Should().Be(value);

            // - wire data
            string.Join("-", buffer.Select(b => b.ToString("X2"))).Should().Be(expectedBytes);

            // - equality
            recdMsg.Should().NotBeNull();
            recdMsg.Should().Be(sendMsg);
            recdMsg!.Equals(sendMsg).Should().BeTrue();
        }

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.NegUnit, "92-C0-FF")]
        [InlineData(ValueKind.MaxValue, "92-C0-7F")]
        [InlineData(ValueKind.MinValue, "92-C0-D0-80")]
        public void Roundtrip_SByte(ValueKind kind, string expectedBytes)
        {
            SByte value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.NegUnit => -1,
                ValueKind.MaxValue => SByte.MaxValue,
                ValueKind.MinValue => SByte.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<SByte, Models.Basics.MessagePack.Data_SByte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.MaxValue, "92-C0-CC-FF")]
        public void Roundtrip_Byte(ValueKind kind, string expectedBytes)
        {
            Byte value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.MaxValue => Byte.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Byte, Models.Basics.MessagePack.Data_Byte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.NegUnit, "92-C0-FF")]
        [InlineData(ValueKind.MaxValue, "92-C0-CD-7F-FF")]
        [InlineData(ValueKind.MinValue, "92-C0-D1-80-00")]
        public void Roundtrip_Int16(ValueKind kind, string expectedBytes)
        {
            Int16 value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.NegUnit => -1,
                ValueKind.MaxValue => Int16.MaxValue,
                ValueKind.MinValue => Int16.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Int16, Models.Basics.MessagePack.Data_Int16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.MaxValue, "92-C0-CD-FF-FF")]
        public void Roundtrip_UInt16(ValueKind kind, string expectedBytes)
        {
            UInt16 value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.MaxValue => UInt16.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<UInt16, Models.Basics.MessagePack.Data_UInt16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.NegUnit, "92-C0-FF")]
        [InlineData(ValueKind.MaxValue, "92-C0-CE-7F-FF-FF-FF")]
        [InlineData(ValueKind.MinValue, "92-C0-D2-80-00-00-00")]
        public void Roundtrip_Int32(ValueKind kind, string expectedBytes)
        {
            Int32 value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.NegUnit => -1,
                ValueKind.MaxValue => Int32.MaxValue,
                ValueKind.MinValue => Int32.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Int32, Models.Basics.MessagePack.Data_Int32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.MaxValue, "92-C0-CE-FF-FF-FF-FF")]
        public void Roundtrip_UInt32(ValueKind kind, string expectedBytes)
        {
            UInt32 value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.MaxValue => UInt32.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<UInt32, Models.Basics.MessagePack.Data_UInt32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.NegUnit, "92-C0-FF")]
        [InlineData(ValueKind.MaxValue, "92-C0-CF-7F-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.MinValue, "92-C0-D3-80-00-00-00-00-00-00-00")]
        public void Roundtrip_Int64(ValueKind kind, string expectedBytes)
        {
            Int64 value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.NegUnit => -1,
                ValueKind.MaxValue => Int64.MaxValue,
                ValueKind.MinValue => Int64.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Int64, Models.Basics.MessagePack.Data_Int64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.MaxValue, "92-C0-CF-FF-FF-FF-FF-FF-FF-FF-FF")]
        public void Roundtrip_UInt64(ValueKind kind, string expectedBytes)
        {
            UInt64 value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.MaxValue => UInt64.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<UInt64, Models.Basics.MessagePack.Data_UInt64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

    }
}