using DataFac.Memory;
using Shouldly;
using MessagePack;
using System;
using System.Threading.Tasks;
using Xunit;
using DTOMaker.Runtime.MemBlocks;

namespace DTOMakerV10.Tests
{
    public class ObjectGraphTests
    {
        [Fact]
        public void RoundTripViaMessagePack()
        {
#if NET8_0_OR_GREATER
            var orig = new DTOMakerV10.Models3.CSRecord.Tree()
            {
                Left = new Models3.CSRecord.Tree()
                {
                    Node = new Models3.CSRecord.DoubleNode() { Key = "Double", Value = 123.456D },
                    Size = 1,
                },
                Right = new Models3.CSRecord.Tree()
                {
                    Node = new Models3.CSRecord.BooleanNode() { Key = "Boolean", Value = true },
                    Size = 1,
                },
                Node = new Models3.CSRecord.StringNode() { Key = "String", Value = "abcdef" },
                Size = 3,
            };
#else
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
#endif

            DTOMakerV10.Models3.MessagePack.Tree sender = DTOMakerV10.Models3.MessagePack.Tree.CreateFrom(orig);
            sender.Freeze();

            ReadOnlyMemory<byte> buffer = MessagePackSerializer.Serialize<DTOMakerV10.Models3.MessagePack.Tree>(sender);
            DTOMakerV10.Models3.MessagePack.Tree recver = MessagePackSerializer.Deserialize<DTOMakerV10.Models3.MessagePack.Tree>(buffer);
            recver.Freeze();

#if NET8_0_OR_GREATER
            var copy = DTOMakerV10.Models3.CSRecord.Tree.CreateFrom(recver);
            copy.ShouldNotBeNull();
            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();
#else
            var copy = DTOMakerV10.Models3.CSPoco.Tree.CreateFrom(recver);
            copy.Freeze();
            copy.ShouldNotBeNull();
            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();
#endif
        }

        [Fact]
        public async Task RoundTripViaMemBlocks()
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

#if NET8_0_OR_GREATER
            var orig = new DTOMakerV10.Models3.CSRecord.Tree()
            {
                Left = new Models3.CSRecord.Tree()
                {
                    Node = new Models3.CSRecord.DoubleNode() { Key = "Double", Value = Double.Epsilon },
                    Size = 1,
                },
                Right = new Models3.CSRecord.Tree()
                {
                    Node = new Models3.CSRecord.BooleanNode() { Key = "Boolean", Value = true },
                    Size = 1,
                },
                Node = new Models3.CSRecord.StringNode() { Key = "String", Value = "abcdef" },
                Size = 3,
            };
#else
            var orig = new DTOMakerV10.Models3.CSPoco.Tree()
            {
                Left = new Models3.CSPoco.Tree()
                {
                    Node = new Models3.CSPoco.DoubleNode() { Key = "Double", Value = Double.Epsilon },
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
#endif

            DTOMakerV10.Models3.MemBlocks.Tree sender = DTOMakerV10.Models3.MemBlocks.Tree.CreateFrom(orig);
            await sender.Pack(dataStore);

            var buffers = sender.GetBuffers();

            DTOMakerV10.Models3.MemBlocks.Tree recver = DTOMakerV10.Models3.MemBlocks.Tree.CreateFrom(buffers);
            await recver.UnpackAll(dataStore);

            recver.Equals(sender).ShouldBeTrue();
            recver.ShouldBe(sender);
            recver.GetHashCode().ShouldBe(sender.GetHashCode());

#if NET8_0_OR_GREATER
            var copy = DTOMakerV10.Models3.CSRecord.Tree.CreateFrom(recver);
            copy.ShouldNotBeNull();
            copy.Equals(orig).ShouldBeTrue();
            copy.ShouldBe(orig);
            copy.GetHashCode().ShouldBe(orig.GetHashCode());
#else
            var copy = DTOMakerV10.Models3.CSPoco.Tree.CreateFrom(recver);
            copy.Freeze();
            copy.ShouldNotBeNull();
            copy.Equals(orig).ShouldBeTrue();
            copy.ShouldBe(orig);
            copy.GetHashCode().ShouldBe(orig.GetHashCode());
#endif
        }

        [Fact]
        public async Task RoundTripViaMemBlocks2_StringNode()
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            var orig = new Models3.CSPoco.StringNode() { Key = "String", Value = "abcdef" };
            orig.Freeze();

            var sender = DTOMakerV10.Models3.MemBlocks.StringNode.CreateFrom(orig);
            await sender.Pack(dataStore);

            var buffers = sender.GetBuffers();

            var recver = DTOMakerV10.Models3.MemBlocks.StringNode.CreateFrom(buffers);
            await recver.UnpackAll(dataStore);

            recver.Equals(sender).ShouldBeTrue();
            recver.ShouldBe(sender);
            recver.GetHashCode().ShouldBe(sender.GetHashCode());

            var copy3 = DTOMakerV10.Models3.CSPoco.StringNode.CreateFrom(recver);
            copy3.Freeze();
            copy3.ShouldNotBeNull();
            copy3.Equals(orig).ShouldBeTrue();
            copy3.ShouldBe(orig);
            copy3.GetHashCode().ShouldBe(orig.GetHashCode());
        }

        [Fact]
        public async Task RoundTripViaMemBlocks3_Int64Node()
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            var orig = new Models3.CSPoco.Int64Node() { Key = "Int64", Value = 257L };
            orig.Freeze();

            var sender = DTOMakerV10.Models3.MemBlocks.Int64Node.CreateFrom(orig);
            await sender.Pack(dataStore);

            var buffers = sender.GetBuffers();

            var recver = DTOMakerV10.Models3.MemBlocks.Int64Node.CreateFrom(buffers);
            await recver.UnpackAll(dataStore);

            recver.Equals(sender).ShouldBeTrue();
            recver.ShouldBe(sender);
            recver.GetHashCode().ShouldBe(sender.GetHashCode());

            var copy3 = DTOMakerV10.Models3.CSPoco.Int64Node.CreateFrom(recver);
            copy3.Freeze();
            copy3.ShouldNotBeNull();
            copy3.Equals(orig).ShouldBeTrue();
            copy3.ShouldBe(orig);
            copy3.GetHashCode().ShouldBe(orig.GetHashCode());
        }

        [Fact]
        public async Task CheckSourceBlocks()
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            var orig = new Models3.CSPoco.Int64Node() { Key = "Int64", Value = 257L };
            orig.Freeze();

            var copy1 = DTOMakerV10.Models3.CSPoco.Int64Node.CreateFrom(orig);
            copy1.Freeze();
            copy1.ShouldNotBeNull();
            copy1.Equals(orig).ShouldBeTrue();
            copy1.ShouldBe(orig);
            copy1.GetHashCode().ShouldBe(orig.GetHashCode());

            var sender = DTOMakerV10.Models3.MemBlocks.Int64Node.CreateFrom(orig);
            await sender.Pack(dataStore);

            var copy2 = DTOMakerV10.Models3.CSPoco.Int64Node.CreateFrom(sender);
            copy2.Freeze();
            copy2.ShouldNotBeNull();
            copy2.Equals(orig).ShouldBeTrue();
            copy2.ShouldBe(orig);
            copy2.GetHashCode().ShouldBe(orig.GetHashCode());

            var buffers = sender.GetBuffers();
            SourceBlocks blocks = SourceBlocks.ParseFrom(buffers);

            blocks.Header.Memory.Span[0].ShouldBe<byte>(0x7C);
            blocks.Header.Memory.Span[1].ShouldBe<byte>(0x5F);
            blocks.Header.Memory.Span[2].ShouldBe<byte>(0x01);
            blocks.Header.Memory.Span[3].ShouldBe<byte>(0x01);
            blocks.Header.SignatureBits.ShouldBe(0x01015F7C);
            blocks.Header.StructureBits.ShouldBe(0x00004053);
            blocks.Header.EntityId.ShouldBe(4);
            blocks.ClassHeight.ShouldBe(3);
            blocks.Blocks.Span[0].Length.ShouldBe(16);
            blocks.Blocks.Span[1].Length.ShouldBe(16);
            blocks.Blocks.Span[2].Length.ShouldBe(0);
            blocks.Blocks.Span[3].Length.ShouldBe(8);
        }

        [Fact]
        public void RoundTripViaJsonNewtonSoft()
        {
            //todo
        }

    }
}