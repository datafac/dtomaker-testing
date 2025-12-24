using System.Threading.Tasks;

namespace Benchmarks.Tests
{
    public class RoundtripBasicsTests
    {
        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16)]
        [InlineData(ValueKind.PairOfInt32)]
        [InlineData(ValueKind.PairOfInt64)]
        public void Roundtrip_MemoryPack(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_MemoryPack();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16)]
        [InlineData(ValueKind.PairOfInt32)]
        [InlineData(ValueKind.PairOfInt64)]
        public void Roundtrip_MsgPack2(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_MsgPack2();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16)]
        [InlineData(ValueKind.PairOfInt32)]
        [InlineData(ValueKind.PairOfInt64)]
        public void Roundtrip_JsonSystemText(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_JsonSystemText();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16)]
        [InlineData(ValueKind.PairOfInt32)]
        [InlineData(ValueKind.PairOfInt64)]
        public void Roundtrip_JsonNewtonSoft(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_JsonNewtonSoft();
        }

        //[Theory]
        //[InlineData(ValueKind.Bool)]
        //[InlineData(ValueKind.DoubleLE)]
        //[InlineData(ValueKind.Guid)]
        //[InlineData(ValueKind.PairOfInt16)]
        //[InlineData(ValueKind.PairOfInt32)]
        //[InlineData(ValueKind.PairOfInt64)]
        //public void Roundtrip_MessagePack(ValueKind valueKind)
        //{
        //    var sut = new DTORoundtripBasics();
        //    sut.CheckValues = true;
        //    sut.Kind = valueKind;
        //    sut.Roundtrip_MessagePack();
        //}

        //[Theory]
        //[InlineData(ValueKind.Bool)]
        //[InlineData(ValueKind.DoubleLE)]
        //[InlineData(ValueKind.Guid)]
        //[InlineData(ValueKind.PairOfInt16)]
        //[InlineData(ValueKind.PairOfInt32)]
        //[InlineData(ValueKind.PairOfInt64)]
        //public async Task Roundtrip_MemBlocks(ValueKind valueKind)
        //{
        //    var sut = new DTORoundtripBasics();
        //    sut.CheckValues = true;
        //    sut.Kind = valueKind;
        //    await sut.Roundtrip_MemBlocks();
        //}
    }
}