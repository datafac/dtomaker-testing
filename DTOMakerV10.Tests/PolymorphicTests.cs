using Shouldly;
using MessagePack;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class PolymorphicTests
    {
        [Fact]

        public async Task RoundTripViaMemBlocks()
        {
            var orig = new DTOMakerV10.Models2.CSPoco.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };

            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            DTOMakerV10.Models2.MemBlocks.Polygon sender = DTOMakerV10.Models2.MemBlocks.Polygon.CreateFrom(orig);
            await sender.Pack(dataStore);

            var buffers = sender.GetBuffers();
            DTOMakerV10.Models2.MemBlocks.Polygon recver = DTOMakerV10.Models2.MemBlocks.Polygon.CreateFrom(buffers);
            recver.IsFrozen.ShouldBeTrue();

            var copy = DTOMakerV10.Models2.CSPoco.Polygon.CreateFrom(recver);
            copy.Freeze();
            copy.ShouldNotBeNull();
            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void RoundTripViaMessagePack()
        {
            var orig = new DTOMakerV10.Models2.CSPoco.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };

            DTOMakerV10.Models2.MessagePack.Polygon sender = DTOMakerV10.Models2.MessagePack.Polygon.CreateFrom(orig);
            sender.Freeze();

            ReadOnlyMemory<byte> buffer = MessagePackSerializer.Serialize<DTOMakerV10.Models2.MessagePack.Polygon>(sender);
            DTOMakerV10.Models2.MessagePack.Polygon recver = MessagePackSerializer.Deserialize<DTOMakerV10.Models2.MessagePack.Polygon>(buffer);
            recver.Freeze();

            var copy = DTOMakerV10.Models2.CSPoco.Polygon.CreateFrom(recver);
            copy.Freeze();
            copy.ShouldNotBeNull();
            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();
        }
    }
}