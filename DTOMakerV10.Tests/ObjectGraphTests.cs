using DataFac.Memory;
using FluentAssertions;
using MessagePack;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class ObjectGraphTests
    {
        [Fact]
        public void RoundTripViaMessagePack()
        {
            var orig = new DTOMakerV10.Models3.CSPoco.Tree()
            {
                Left = new Models3.CSPoco.Tree()
                {
                    Node = new Models3.CSPoco.DoubleNode() { Key = "Double", Value = 123.456D },
                    Size = 1,
                },
                Right = new Models3.CSPoco.Tree()
                {
                    Node = new Models3.CSPoco.BooleanNode() { Key = "Boolean", Value = true },
                    Size = 1,
                },
                Node = new Models3.CSPoco.StringNode() { Key = "String", Value = "abcdef" },
                Size = 3,
            };
            orig.Freeze();

            DTOMakerV10.Models3.MessagePack.Tree sender = DTOMakerV10.Models3.MessagePack.Tree.CreateFrom(orig);
            sender.Freeze();

            ReadOnlyMemory<byte> buffer = MessagePackSerializer.Serialize<DTOMakerV10.Models3.MessagePack.Tree>(sender);
            DTOMakerV10.Models3.MessagePack.Tree recver = MessagePackSerializer.Deserialize<DTOMakerV10.Models3.MessagePack.Tree>(buffer);
            recver.Freeze();

            var copy = DTOMakerV10.Models3.CSPoco.Tree.CreateFrom(recver);
            copy.Freeze();
            copy.Should().NotBeNull();
            copy.Should().Be(orig);
            copy.Equals(orig).Should().BeTrue();
        }
        [Fact]
        public async Task RoundTripViaMemBlocks()
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            var orig = new DTOMakerV10.Models3.CSPoco.Tree()
            {
                Left = new Models3.CSPoco.Tree()
                {
                    Node = new Models3.CSPoco.DoubleNode() { Key = "Double", Value = Double.NaN },
                    Size = 1,
                },
                Right = new Models3.CSPoco.Tree()
                {
                    Node = new Models3.CSPoco.BooleanNode() { Key = "Boolean", Value = true },
                    Size = 1,
                },
                Node = new Models3.CSPoco.StringNode() { Key = "String", Value = "abcdef" },
                Size = 3,
            };
            orig.Freeze();

            DTOMakerV10.Models3.MemBlocks.Tree sender = DTOMakerV10.Models3.MemBlocks.Tree.CreateFrom(orig);
            await sender.Pack(dataStore);
            //sender.Freeze();

            ReadOnlyMemory<byte> buffer = sender.GetBuffer();

            DTOMakerV10.Models3.MemBlocks.Tree recver = DTOMakerV10.Models3.MemBlocks.Tree.CreateFrom(buffer);
            //recver.Freeze();
            await recver.UnpackAll(dataStore);

            recver.Equals(sender).Should().BeTrue();
            recver.Should().Be(sender);
            recver.GetHashCode().Should().Be(sender.GetHashCode());

            var copy = DTOMakerV10.Models3.CSPoco.Tree.CreateFrom(recver);
            copy.Freeze();
            copy.Should().NotBeNull();
            copy.Equals(orig).Should().BeTrue();
            copy.Should().Be(orig);
            copy.GetHashCode().Should().Be(orig.GetHashCode());
        }
    }
}