using DataFac.Runtime;
using DTOMakerV10.Models.MessagePack;
using FluentAssertions;
using MessagePack;
using System;
using System.Linq;
using Xunit;

namespace DTOMakerV10.Tests
{
    public enum ValueKind
    {
        Default,
        Epsilon,
        PosUnit,
        NegUnit,
        MaxValue,
        MinValue,
    }

    internal interface IDataHelper<TValue, TClass>
    {
        TValue CreateValue(ValueKind kind);
        TClass NewClass(TValue value);
        TValue GetValue(TClass message);
    }

    internal class DataHelper_Int32 : IDataHelper<Int32, Data_Int32>
    {
        public static readonly DataHelper_Int32 Instance = new DataHelper_Int32();

        public Int32 CreateValue(ValueKind kind)
        {
            return kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.NegUnit => -1,
                ValueKind.MaxValue => Int32.MaxValue,
                ValueKind.MinValue => Int32.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
        }

        public int GetValue(Data_Int32 message) => message.Value;
        public Data_Int32 NewClass(Int32 value) => new Data_Int32 { Value = value };
    }
    public class MyClassTests
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
    }
}