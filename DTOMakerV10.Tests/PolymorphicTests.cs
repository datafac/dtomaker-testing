﻿using FluentAssertions;
using MessagePack;
using System;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class PolymorphicTests
    {
        [Fact]
        public void RoundTripViaMemBlocks()
        {
            var orig = new DTOMakerV10.Models2.CSPoco.Rectangle()
            {
                Length = 3.0D,
                Height = 2.0D,
            };

            DTOMakerV10.Models2.MemBlocks.Polygon sender = DTOMakerV10.Models2.MemBlocks.Polygon.CreateFrom(orig);
            sender.Freeze();

            var entityId = sender.GetEntityId();
            var buffers = sender.GetBuffers();
            DTOMakerV10.Models2.MemBlocks.Polygon recver = DTOMakerV10.Models2.MemBlocks.Polygon.CreateFrom(entityId, buffers);
            recver.IsFrozen.Should().BeTrue();

            var copy = DTOMakerV10.Models2.CSPoco.Polygon.CreateFrom(recver);
            copy.Freeze();
            copy.Should().NotBeNull();
            copy.Should().Be(orig);
            copy.Equals(orig).Should().BeTrue();
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
            copy.Should().NotBeNull();
            copy.Should().Be(orig);
            copy.Equals(orig).Should().BeTrue();
        }
    }
}