using DataFac.Storage;
using Shouldly;
using System.Threading.Tasks;

namespace Benchmarks.Tests
{
    public class RoundtripBinaryTreeTests
    {
        [Fact]
        public void BinaryTree_MemoryPack()
        {
            var sut = new DTORoundtripBinaryTree();
            sut.CheckValues = true;
            sut.BinaryTree_MemoryPack();
        }

        [Fact]
        public async Task BinaryTree_MemBlocks()
        {
            var sut = new DTORoundtripBinaryTree();
            sut.CheckValues = true;
            sut.ResetCounters();
            await sut.BinaryTree_MemBlocks();

            //Counters counters = sut.GetCounters();
            //counters.BlobPutCount.ShouldBe(2);
            //counters.BlobPutSkips.ShouldBe(0);
            //counters.BlobPutWrits.ShouldBe(2);
            //counters.ByteDelta.ShouldBe(544);
        }

        [Fact]
        public void BinaryTree_MsgPack2()
        {
            var sut = new DTORoundtripBinaryTree();
            sut.CheckValues = true;
            sut.BinaryTree_MsgPack2();
        }

        [Fact]
        public void BinaryTree_JsonSystemText()
        {
            var sut = new DTORoundtripBinaryTree();
            sut.CheckValues = true;
            sut.BinaryTree_JsonSystemText();
        }

        [Fact]
        public void BinaryTree_JsonNewtonSoft()
        {
            var sut = new DTORoundtripBinaryTree();
            sut.CheckValues = true;
            sut.BinaryTree_JsonNewtonSoft();
        }
    }
}