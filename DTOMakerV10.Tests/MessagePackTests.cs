using DataFac.Runtime;
using FluentAssertions;
using MessagePack;
using System.Linq;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class MessagePackTests
    {
        private void Roundtrip<TClass, TValue>(IDataHelper<TValue, TClass>  helper, ValueKind kind, string expectedBytes)
            where TClass : IFreezable
        {
            // arrange
            TValue origValue = helper.CreateValue(kind);
            TClass sendMsg = helper.NewClass(origValue);
            sendMsg.Freeze();

            // act
            var buffer = MessagePackSerializer.Serialize<TClass>(sendMsg);
            var recdMsg = MessagePackSerializer.Deserialize<TClass>(buffer);

            // assert
            // - value
            TValue copyValue = helper.GetValue(recdMsg);
            copyValue.Should().Be(origValue);

            // - wire data
            string.Join("-", buffer.Select(b => b.ToString("X2"))).Should().Be(expectedBytes);

            // - equality
            recdMsg.Should().Be(sendMsg);
            recdMsg.Equals(sendMsg).Should().BeTrue();
        }

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.NegUnit, "92-C0-FF")]
        [InlineData(ValueKind.MaxValue, "92-C0-CE-7F-FF-FF-FF")]
        [InlineData(ValueKind.MinValue, "92-C0-D2-80-00-00-00")]
        public void Roundtrip_Int32(ValueKind kind, string expectedBytes) => Roundtrip(DataHelper_Int32.Instance, kind, expectedBytes);

        [Theory]
        [InlineData(ValueKind.Default, "92-C0-00")]
        [InlineData(ValueKind.PosUnit, "92-C0-01")]
        [InlineData(ValueKind.NegUnit, "92-C0-FF")]
        [InlineData(ValueKind.MaxValue, "92-C0-CD-7F-FF")]
        [InlineData(ValueKind.MinValue, "92-C0-D1-80-00")]
        public void Roundtrip_Int16(ValueKind kind, string expectedBytes) => Roundtrip(DataHelper_Int16.Instance, kind, expectedBytes);

    }
}