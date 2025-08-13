using Shouldly;
using MessagePack;
using System;
using System.Threading.Tasks;
using Xunit;
using DTOMaker.Runtime.MemBlocks;
using Newtonsoft.Json;
using DTOMaker.Runtime.MessagePack;

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

            var buffer = sender.SerializeToMessagePack<DTOMakerV10.Models3.MessagePack.Tree>();
            var recver = buffer.DeserializeFromMessagePack<DTOMakerV10.Models3.MessagePack.Tree>();
            recver.Freeze();

            var copy = DTOMakerV10.Models3.CSPoco.Tree.CreateFrom(recver);
            copy.Freeze();
            copy.ShouldNotBeNull();
            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public async Task RoundTripViaMemBlocks()
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

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

            DTOMakerV10.Models3.MemBlocks.Tree sender = DTOMakerV10.Models3.MemBlocks.Tree.CreateFrom(orig);
            await sender.Pack(dataStore);

            var buffers = sender.GetBuffers();

            DTOMakerV10.Models3.MemBlocks.Tree recver = DTOMakerV10.Models3.MemBlocks.Tree.CreateFrom(buffers);
            await recver.UnpackAll(dataStore);

            recver.Equals(sender).ShouldBeTrue();
            recver.ShouldBe(sender);
            recver.GetHashCode().ShouldBe(sender.GetHashCode());

            var copy = DTOMakerV10.Models3.CSPoco.Tree.CreateFrom(recver);
            copy.Freeze();
            copy.ShouldNotBeNull();
            copy.Equals(orig).ShouldBeTrue();
            copy.ShouldBe(orig);
            copy.GetHashCode().ShouldBe(orig.GetHashCode());
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

        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
        };

        [Fact]

        public async Task RoundTripViaJsonNewtonSoft()
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

            DTOMakerV10.Models3.JsonNewtonSoft.Tree sender = DTOMakerV10.Models3.JsonNewtonSoft.Tree.CreateFrom(orig);
            sender.Freeze();

            string buffer = JsonConvert.SerializeObject(sender, typeof(DTOMakerV10.Models3.JsonNewtonSoft.Tree), jsonSettings);

            await VerifyXunit.Verifier.Verify(buffer);

            DTOMakerV10.Models3.JsonNewtonSoft.Tree? recver = JsonConvert.DeserializeObject<DTOMakerV10.Models3.JsonNewtonSoft.Tree>(buffer, jsonSettings);

            recver.ShouldNotBeNull();
            recver.Freeze();

            var copy = DTOMakerV10.Models3.CSPoco.Tree.CreateFrom(recver);
            copy.Freeze();
            copy.ShouldNotBeNull();
            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();
        }

    }
}