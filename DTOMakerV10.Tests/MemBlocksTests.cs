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
        }

        [Theory]
        [InlineData(ValueKind.Default, "00")]
        [InlineData(ValueKind.PosUnit, "01")]
        [InlineData(ValueKind.NegUnit, "FF")]
        [InlineData(ValueKind.MaxValue, "7F")]
        [InlineData(ValueKind.MinValue, "80")]
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

            Roundtrip<SByte, Data_SByte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "00")]
        [InlineData(ValueKind.PosUnit, "01")]
        [InlineData(ValueKind.MaxValue, "FF")]
        public void Roundtrip_Byte(ValueKind kind, string expectedBytes)
        {
            Byte value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.MaxValue => Byte.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<Byte, Data_Byte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "00-00")]
        [InlineData(ValueKind.PosUnit, "01-00")]
        [InlineData(ValueKind.NegUnit, "FF-FF")]
        [InlineData(ValueKind.MaxValue, "FF-7F")]
        [InlineData(ValueKind.MinValue, "00-80")]
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

            Roundtrip<Int16, Data_Int16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "00-00")]
        [InlineData(ValueKind.PosUnit, "01-00")]
        [InlineData(ValueKind.MaxValue, "FF-FF")]
        public void Roundtrip_UInt16(ValueKind kind, string expectedBytes)
        {
            UInt16 value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.MaxValue => UInt16.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<UInt16, Data_UInt16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "00-00-00-00")]
        [InlineData(ValueKind.PosUnit, "01-00-00-00")]
        [InlineData(ValueKind.NegUnit, "FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxValue, "FF-FF-FF-7F")]
        [InlineData(ValueKind.MinValue, "00-00-00-80")]
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

            Roundtrip<Int32, Data_Int32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "00-00-00-00")]
        [InlineData(ValueKind.PosUnit, "01-00-00-00")]
        [InlineData(ValueKind.MaxValue, "FF-FF-FF-FF")]
        public void Roundtrip_UInt32(ValueKind kind, string expectedBytes)
        {
            UInt32 value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.MaxValue => UInt32.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<UInt32, Data_UInt32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosUnit, "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.NegUnit, "FF-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(ValueKind.MaxValue, "FF-FF-FF-FF-FF-FF-FF-7F")]
        [InlineData(ValueKind.MinValue, "00-00-00-00-00-00-00-80")]
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

            Roundtrip<Int64, Data_Int64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "00-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.PosUnit, "01-00-00-00-00-00-00-00")]
        [InlineData(ValueKind.MaxValue, "FF-FF-FF-FF-FF-FF-FF-FF")]
        public void Roundtrip_UInt64(ValueKind kind, string expectedBytes)
        {
            UInt64 value = kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.MaxValue => UInt64.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<UInt64, Data_UInt64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.Default, "00")]
        [InlineData(ValueKind.PosUnit, "01")]
        public void Roundtrip_Boolean(ValueKind kind, string expectedBytes)
        {
            Boolean value = kind switch
            {
                ValueKind.Default => false,
                ValueKind.PosUnit => true,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip<Boolean, Data_Boolean>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        // todo Guid, decimal, half, float, double, int128, uint128

    }
}
