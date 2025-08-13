using Shouldly;
using MessagePack;
using System;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using DTOMaker.Runtime.MessagePack;

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

            var buffer = sender.SerializeToMessagePack<DTOMakerV10.Models2.MessagePack.Polygon>();
            var recver = buffer.DeserializeFromMessagePack<DTOMakerV10.Models2.MessagePack.Polygon>();
            recver.Freeze();

            var copy = DTOMakerV10.Models2.CSPoco.Polygon.CreateFrom(recver);
            copy.Freeze();
            copy.ShouldNotBeNull();
            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();
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
            var orig = new DTOMakerV10.Models2.CSPoco.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };

            DTOMakerV10.Models2.JsonNewtonSoft.Polygon sender = DTOMakerV10.Models2.JsonNewtonSoft.Polygon.CreateFrom(orig);
            sender.Freeze();
            sender.ShouldBeOfType<DTOMakerV10.Models2.JsonNewtonSoft.Rectangle>();

            string buffer = JsonConvert.SerializeObject(sender, typeof(DTOMakerV10.Models2.JsonNewtonSoft.Polygon), jsonSettings);

            await VerifyXunit.Verifier.Verify(buffer);

            DTOMakerV10.Models2.JsonNewtonSoft.Polygon? recver = JsonConvert.DeserializeObject<DTOMakerV10.Models2.JsonNewtonSoft.Polygon>(buffer, jsonSettings);

            recver.ShouldNotBeNull();
            recver.Freeze();
            recver.ShouldBeOfType<DTOMakerV10.Models2.JsonNewtonSoft.Rectangle>();

            var copy = DTOMakerV10.Models2.CSPoco.Polygon.CreateFrom(recver);
            copy.Freeze();
            copy.ShouldNotBeNull();
            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();
        }

    }
}