using MessagePack;
using Sandbox.Generics.Models.MemBlocks;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sandbox.Generics.Tests
{
    public class GenericsTests_MemBlocks
    {
        [Fact]
        public async Task Roundtrip1_SimpleAsync()
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            var sendMsg = new MyTree()
            {
                Count = 1,
                Key = "abc",
                Value = 456L,
                Left = null,
                Right = null,
            };

            // act
            await sendMsg.Pack(dataStore);
            var buffers = sendMsg.GetBuffers();
            MyTree recdMsg = new MyTree(buffers);
            await recdMsg.UnpackAll(dataStore);

            // assert
            // - equality
            recdMsg.ShouldNotBeNull();
            recdMsg!.Equals(sendMsg).ShouldBeTrue();
            recdMsg.ShouldBe(sendMsg);
            recdMsg.GetHashCode().ShouldBe(sendMsg.GetHashCode());
        }
    }
}
