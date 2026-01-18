using System.Threading.Tasks;

namespace Benchmarks.Tests
{
    public class RoundtripCustomTests
    {
        [Fact]
        public void Roundtrip_MemoryPack()
        {
            var sut = new DTORoundtripCustom1();
            sut.CheckValues = true;
            sut.Roundtrip_MemoryPack();
        }

        [Fact]
        public void Roundtrip_MsgPack2()
        {
            var sut = new DTORoundtripCustom1();
            sut.CheckValues = true;
            sut.Roundtrip_MsgPack2();
        }

        [Fact]
        public async Task Roundtrip_MemBlocks()
        {
            var sut = new DTORoundtripCustom1();
            sut.CheckValues = true;
            await sut.Roundtrip_MemBlocks();
        }

        [Fact]
        public void Roundtrip_JsonSystemText()
        {
            var sut = new DTORoundtripCustom1();
            sut.CheckValues = true;
            sut.Roundtrip_JsonSystemText();
        }

        [Fact]
        public void Roundtrip_JsonNewtonSoft()
        {
            var sut = new DTORoundtripCustom1();
            sut.CheckValues = true;
            sut.Roundtrip_JsonNewtonSoft();
        }
    }
}