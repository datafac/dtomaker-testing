using System.Threading.Tasks;

namespace Benchmarks.Tests
{

    public class RoundtripBasicsTests
    {
        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.Int32LE)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16)]
        [InlineData(ValueKind.PairOfInt32)]
        [InlineData(ValueKind.PairOfInt64)]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        [InlineData(ValueKind.BinaryNull)]
        [InlineData(ValueKind.BinaryEmpty)]
        [InlineData(ValueKind.BinarySmall)]
        [InlineData(ValueKind.BinaryLarge)]
        [InlineData(ValueKind.AllPropsSet)]
        public void Roundtrip_MemoryPack(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_MemoryPack();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.Int32LE)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16)]
        [InlineData(ValueKind.PairOfInt32)]
        [InlineData(ValueKind.PairOfInt64)]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        [InlineData(ValueKind.BinaryNull)]
        [InlineData(ValueKind.BinaryEmpty)]
        [InlineData(ValueKind.BinarySmall)]
        [InlineData(ValueKind.BinaryLarge)]
        [InlineData(ValueKind.AllPropsSet)]
        public void Roundtrip_MsgPack2(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_MsgPack2();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.Int32LE)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16)]
        [InlineData(ValueKind.PairOfInt32)]
        [InlineData(ValueKind.PairOfInt64)]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        [InlineData(ValueKind.BinaryNull)]
        [InlineData(ValueKind.BinaryEmpty)]
        [InlineData(ValueKind.BinarySmall)]
        [InlineData(ValueKind.BinaryLarge)]
        [InlineData(ValueKind.AllPropsSet)]
        public async Task Roundtrip_MemBlocks(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            await sut.Roundtrip_MemBlocks();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.Int32LE)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16)]
        [InlineData(ValueKind.PairOfInt32)]
        [InlineData(ValueKind.PairOfInt64)]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        [InlineData(ValueKind.BinaryNull)]
        [InlineData(ValueKind.BinaryEmpty)]
        [InlineData(ValueKind.BinarySmall)]
        [InlineData(ValueKind.BinaryLarge)]
        [InlineData(ValueKind.AllPropsSet)]
        public void Roundtrip_JsonSystemText(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_JsonSystemText();
        }

        [Theory]
        [InlineData(ValueKind.Bool)]
        [InlineData(ValueKind.Int32LE)]
        [InlineData(ValueKind.DoubleLE)]
        [InlineData(ValueKind.Guid)]
        [InlineData(ValueKind.PairOfInt16)]
        [InlineData(ValueKind.PairOfInt32)]
        [InlineData(ValueKind.PairOfInt64)]
        [InlineData(ValueKind.StringNull)]
        [InlineData(ValueKind.StringEmpty)]
        [InlineData(ValueKind.StringSmall)]
        [InlineData(ValueKind.StringLarge)]
        [InlineData(ValueKind.BinaryNull)]
        [InlineData(ValueKind.BinaryEmpty)]
        [InlineData(ValueKind.BinarySmall)]
        [InlineData(ValueKind.BinaryLarge)]
        [InlineData(ValueKind.AllPropsSet)]
        public void Roundtrip_JsonNewtonSoft(ValueKind valueKind)
        {
            var sut = new DTORoundtripBasics();
            sut.CheckValues = true;
            sut.Kind = valueKind;
            sut.Roundtrip_JsonNewtonSoft();
        }
    }
}