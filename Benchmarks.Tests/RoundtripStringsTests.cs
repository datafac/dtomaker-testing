using System.Threading.Tasks;

namespace Benchmarks.Tests
{

    public class RoundtripStringsTests
    {
        [Theory]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringZero)]
        [InlineData(ValueKind.StringFull)]
        public void Roundtrip_MessagePack(ValueKind valueKind)
        {
            var sut = new DTORoundtripStrings();
            sut.Kind = valueKind;
            sut.Roundtrip_MessagePack();
        }

        [Theory]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringZero)]
        [InlineData(ValueKind.StringFull)]
        public void Roundtrip_MemoryPack(ValueKind valueKind)
        {
            var sut = new DTORoundtripStrings();
            sut.Kind = valueKind;
            sut.Roundtrip_MemoryPack();
        }

        [Theory]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringZero)]
        [InlineData(ValueKind.StringFull)]
        public async Task Roundtrip_MemBlocks(ValueKind valueKind)
        {
            var sut = new DTORoundtripStrings();
            sut.Kind = valueKind;
            int _ = await sut.Roundtrip_MemBlocks();
        }

        [Theory]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringZero)]
        [InlineData(ValueKind.StringFull)]
        public void Roundtrip_NetStrux(ValueKind valueKind)
        {
            var sut = new DTORoundtripStrings();
            sut.Kind = valueKind;
            sut.Roundtrip_NetStrux();
        }

    }
}