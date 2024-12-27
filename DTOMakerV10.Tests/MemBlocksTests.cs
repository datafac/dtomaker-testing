using DataFac.Runtime;
using DTOMakerV10.Models.MemBlocks;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class MemBlocksTests
    {
        private void Roundtrip2<TValue, TMsg>(TValue value, string expectedBytes, Action<TMsg, TValue> setValueFunc, Func<TMsg, TValue> getValueFunc)
            where TMsg : EntityBase, IFreezable, new()
        {
            var sendMsg = new TMsg();
            setValueFunc(sendMsg, value);
            sendMsg.Freeze();

            // act
            var entityId = sendMsg.GetEntityId();
            var buffers = sendMsg.GetBuffers();
            var newBase = EntityBase.CreateFrom(entityId, buffers);
            newBase.Should().NotBeNull();
            newBase.Should().BeOfType<TMsg>();
            TMsg recdMsg = (TMsg)newBase;

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

            Roundtrip2<Int16, Models.MemBlocks.Data_Int16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
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

            Roundtrip2<Int32, Models.MemBlocks.Data_Int32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
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

            Roundtrip2<Int64, Models.MemBlocks.Data_Int64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

    }
}
