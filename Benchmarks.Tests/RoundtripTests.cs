using System.Threading.Tasks;
using Xunit;

namespace Benchmarks.Tests
{

    public class RoundtripStringTests
    {
        [Theory]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        public void Roundtrip_MemoryPack(ValueKind valueKind)
        {
            var sut = new DTORoundtripString();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_MemoryPack();
        }

        [Theory]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        public void Roundtrip_MsgPack2(ValueKind valueKind)
        {
            var sut = new DTORoundtripString();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_MsgPack2();
        }

        [Theory]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        public async Task Roundtrip_MemBlocks(ValueKind valueKind)
        {
            var sut = new DTORoundtripString();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            await sut.Roundtrip_MemBlocks();
        }

        [Theory]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        public void Roundtrip_JsonSystemText(ValueKind valueKind)
        {
            var sut = new DTORoundtripString();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_JsonSystemText();
        }

        [Theory]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        public void Roundtrip_JsonNewtonSoft(ValueKind valueKind)
        {
            var sut = new DTORoundtripString();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_JsonNewtonSoft();
        }
    }
}