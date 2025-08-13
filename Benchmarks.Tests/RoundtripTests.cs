using System.Threading.Tasks;

namespace Benchmarks.Tests
{
    public class RoundtripBasicsTests
    {
        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16qqq)]
        [InlineData(ValueKind.PairOfInt32)]
        public void Roundtrip_MessagePack(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.Kind = valueKind;
            sut.Roundtrip_MessagePack();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16qqq)]
        [InlineData(ValueKind.PairOfInt32)]
        public void Roundtrip_JsonNewtonSoft(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.Kind = valueKind;
            sut.Roundtrip_JsonNewtonSoft();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16qqq)]
        [InlineData(ValueKind.PairOfInt32)]
        public async Task Roundtrip_MemBlocks(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.Kind = valueKind;
            await sut.Roundtrip_MemBlocks();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16qqq)]
        [InlineData(ValueKind.PairOfInt32)]
        public void Roundtrip_MemoryPack(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.Kind = valueKind;
            sut.Roundtrip_MemoryPack();
        }
    }
}