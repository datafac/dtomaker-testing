using System.Threading.Tasks;

namespace Benchmarks.Tests
{
    public class RoundtripPolymorphicTests
    {
        [Fact]
        public void Polymorphic_MemoryPack()
        {
            var sut = new DTORoundtripPolymorphic();
            sut.CheckValues = true;
            sut.Polymorphic_MemoryPack();
        }

        [Fact]
        public void Polymorphic_MsgPack2()
        {
            var sut = new DTORoundtripPolymorphic();
            sut.CheckValues = true;
            sut.Polymorphic_MsgPack2();
        }

        [Fact]
        public void Polymorphic_JsonSystemText()
        {
            var sut = new DTORoundtripPolymorphic();
            sut.CheckValues = true;
            sut.Polymorphic_JsonSystemText();
        }

        [Fact]
        public void Polymorphic_JsonNewtonSoft()
        {
            var sut = new DTORoundtripPolymorphic();
            sut.CheckValues = true;
            sut.Polymorphic_JsonNewtonSoft();
        }
    }
}