using DataFac.Memory;
using DTOMaker.Runtime;
using DTOMaker.Runtime.JsonSystemText;
using Shouldly;
using System;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class JsonSystemTextTests
    {
        private void Roundtrip2<TValue, TMsg>(TValue value, string expectedBuffer, Action<TMsg, TValue> setValueFunc, Func<TMsg, TValue> getValueFunc)
            where TMsg : class, IEntityBase, IEquatable<TMsg>, new()
        {
            var sendMsg = new TMsg();
            setValueFunc(sendMsg, value);
            sendMsg.Freeze();

            // act
            string buffer = sendMsg.SerializeToJson<TMsg>();
            TMsg? recdMsg = buffer.DeserializeFromJson<TMsg>();
            recdMsg.ShouldNotBeNull();
            recdMsg.Freeze();

            // assert
            // - wire data
            buffer.ShouldBe(expectedBuffer);

            // - value
            TValue copyValue = getValueFunc(recdMsg);
            copyValue.ShouldBe(value);

            // - equality
            recdMsg.ShouldNotBeNull();
            recdMsg.Equals(sendMsg).ShouldBeTrue();
            recdMsg.ShouldBe(sendMsg);
            recdMsg.GetHashCode().ShouldBe(sendMsg.GetHashCode());
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":true}")]
        public void Roundtrip_Boolean(ValueKind kind, string expectedBytes)
        {
            Boolean value = kind switch
            {
                ValueKind.DefVal => false,
                ValueKind.PosOne => true,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Boolean, Models.JsonSystemText.Data_Boolean>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":127}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":-1}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":-128}")]
        public void Roundtrip_SByte(ValueKind kind, string expectedBytes)
        {
            SByte value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => SByte.MaxValue,
                ValueKind.MinVal => SByte.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<SByte, Models.JsonSystemText.Data_SByte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":255}")]
        public void Roundtrip_Byte(ValueKind kind, string expectedBytes)
        {
            Byte value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => Byte.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Byte, Models.JsonSystemText.Data_Byte>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":32767}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":-1}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":-32768}")]
        public void Roundtrip_Int16(ValueKind kind, string expectedBytes)
        {
            Int16 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Int16.MaxValue,
                ValueKind.MinVal => Int16.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Int16, Models.JsonSystemText.Data_Int16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":65535}")]
        public void Roundtrip_UInt16(ValueKind kind, string expectedBytes)
        {
            UInt16 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt16.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<UInt16, Models.JsonSystemText.Data_UInt16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":2147483647}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":-1}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":-2147483648}")]
        public void Roundtrip_Int32(ValueKind kind, string expectedBytes)
        {
            Int32 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Int32.MaxValue,
                ValueKind.MinVal => Int32.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Int32, Models.JsonSystemText.Data_Int32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":4294967295}")]
        public void Roundtrip_UInt32(ValueKind kind, string expectedBytes)
        {
            UInt32 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt32.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<UInt32, Models.JsonSystemText.Data_UInt32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":9223372036854775807}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":-1}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":-9223372036854775808}")]
        public void Roundtrip_Int64(ValueKind kind, string expectedBytes)
        {
            Int64 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Int64.MaxValue,
                ValueKind.MinVal => Int64.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Int64, Models.JsonSystemText.Data_Int64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":18446744073709551615}")]
        public void Roundtrip_UInt64(ValueKind kind, string expectedBytes)
        {
            UInt64 value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.MaxVal => UInt64.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<UInt64, Models.JsonSystemText.Data_UInt64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":\" \"}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":\"\\uFFFF\"}")]
        public void Roundtrip_Char(ValueKind kind, string expectedBytes)
        {
            Char value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => ' ',
                ValueKind.MaxVal => Char.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Char, Models.JsonSystemText.Data_Char>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

#if NET8_0_OR_GREATER
        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":-1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":65500}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":-65500}")]
        [InlineData(ValueKind.MinInc, "{\"Value\":6E-08}")]
        [InlineData(ValueKind.NegInf, "{\"Value\":\"-Infinity\"}")]
        [InlineData(ValueKind.PosInf, "{\"Value\":\"Infinity\"}")]
        //[InlineData(ValueKind.NotNum, "{\"Value\":\"NaN\"}")] // todo NaN equality check fails
        public void Roundtrip_Half(ValueKind kind, string expectedBytes)
        {
            Half value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => Half.One,
                ValueKind.NegOne => Half.NegativeOne,
                ValueKind.MaxVal => Half.MaxValue,
                ValueKind.MinVal => Half.MinValue,
                ValueKind.MinInc => Half.Epsilon,
                ValueKind.NegInf => Half.NegativeInfinity,
                ValueKind.PosInf => Half.PositiveInfinity,
                ValueKind.NotNum => Half.NaN,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Half, Models.JsonSystemText.Data_Half>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }
#endif

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":-1}")]
#if NET8_0_OR_GREATER
        [InlineData(ValueKind.MaxVal, "{\"Value\":3.4028235E+38}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":-3.4028235E+38}")]
#else
        [InlineData(ValueKind.MaxVal, "{\"Value\":3.40282347E+38}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":-3.40282347E+38}")]
#endif
        //[InlineData(ValueKind.MinInc, "{\"Value\":1E-45}")] // todo deserialize fails
        [InlineData(ValueKind.NegInf, "{\"Value\":\"-Infinity\"}")]
        [InlineData(ValueKind.PosInf, "{\"Value\":\"Infinity\"}")]
        //[InlineData(ValueKind.NotNum, "{\"Value\":\"NaN\"}")] // todo NaN equality check fails
        public void Roundtrip_Single(ValueKind kind, string expectedBytes)
        {
            Single value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Single.MaxValue,
                ValueKind.MinVal => Single.MinValue,
                ValueKind.MinInc => Single.Epsilon,
                ValueKind.NegInf => Single.NegativeInfinity,
                ValueKind.PosInf => Single.PositiveInfinity,
                ValueKind.NotNum => Single.NaN,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Single, Models.JsonSystemText.Data_Single>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":-1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":1.7976931348623157E+308}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":-1.7976931348623157E+308}")]
#if NET8_0_OR_GREATER
        [InlineData(ValueKind.MinInc, "{\"Value\":5E-324}")]
#else
        [InlineData(ValueKind.MinInc, "{\"Value\":4.9406564584124654E-324}")]
#endif
        [InlineData(ValueKind.NegInf, "{\"Value\":\"-Infinity\"}")]
        [InlineData(ValueKind.PosInf, "{\"Value\":\"Infinity\"}")]
        //[InlineData(ValueKind.NotNum, "{\"Value\":\"NaN\"}")] // todo NaN equality check fails
        public void Roundtrip_Double(ValueKind kind, string expectedBytes)
        {
            Double value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => 1,
                ValueKind.NegOne => -1,
                ValueKind.MaxVal => Double.MaxValue,
                ValueKind.MinVal => Double.MinValue,
                ValueKind.MinInc => Double.Epsilon,
                ValueKind.NegInf => Double.NegativeInfinity,
                ValueKind.PosInf => Double.PositiveInfinity,
                ValueKind.NotNum => Double.NaN,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Double, Models.JsonSystemText.Data_Double>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":1}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":-1}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":79228162514264337593543950335}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":-79228162514264337593543950335}")]
        public void Roundtrip_Decimal(ValueKind kind, string expectedBytes)
        {
            Decimal value = kind switch
            {
                ValueKind.DefVal => default,
                ValueKind.PosOne => Decimal.One,
                ValueKind.NegOne => Decimal.MinusOne,
                ValueKind.MaxVal => Decimal.MaxValue,
                ValueKind.MinVal => Decimal.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Decimal, Models.JsonSystemText.Data_Decimal>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.MinInc, "{\"Value\":\"00000000-0000-0000-0000-000000000001\"}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":\"ffffffff-ffff-ffff-ffff-ffffffffffff\"}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":\"cb38a1ff-0470-4e06-9d88-3461eb5257eb\"}")]
        public void Roundtrip_Guid(ValueKind kind, string expectedBytes)
        {
            Guid value = kind switch
            {
                ValueKind.DefVal => Guid.Empty,
                ValueKind.MinInc => new Guid("00000000-0000-0000-0000-000000000001"),
                ValueKind.MaxVal => new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                ValueKind.PosOne => new Guid("cb38a1ff-0470-4e06-9d88-3461eb5257eb"),
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Guid, Models.JsonSystemText.Data_Guid>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.MinVal, "{\"Value\":\"\"}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":\"abcdef\"}")]
        public void Roundtrip_String(ValueKind kind, string expectedBytes)
        {
            string value = kind switch
            {
                //ValueKind.DefVal => null,
                ValueKind.MinVal => string.Empty,
                ValueKind.PosOne => "abcdef",
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<string, Models.JsonSystemText.Data_String>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.MinVal, "{\"Value\":\"\"}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":\"YWJjZGVm\"}")]
        public void Roundtrip_Octets(ValueKind kind, string expectedBytes)
        {
            Octets value = kind switch
            {
                //ValueKind.DefVal => null,
                ValueKind.MinVal => Octets.Empty,
                ValueKind.PosOne => new Octets(Encoding.UTF8.GetBytes("abcdef")),
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<Octets, Models.JsonSystemText.Data_Octets>(value, expectedBytes, (m, v) => { m.Value = v.ToByteArray(); }, (m) => new Octets(m.Value));
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":{\"A\":1,\"B\":1}}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":{\"A\":-1,\"B\":-1}}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":{\"A\":32767,\"B\":32767}}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":{\"A\":-32768,\"B\":-32768}}")]
        public void Roundtrip_PairOfInt16(ValueKind kind, string expectedBytes)
        {
            PairOfInt16 value = kind switch
            {
                ValueKind.DefVal => new PairOfInt16(default, default),
                ValueKind.PosOne => new PairOfInt16(1, 1),
                ValueKind.NegOne => new PairOfInt16(-1, -1),
                ValueKind.MaxVal => new PairOfInt16(Int16.MaxValue, Int16.MaxValue),
                ValueKind.MinVal => new PairOfInt16(Int16.MinValue, Int16.MinValue),
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<PairOfInt16, Models.JsonSystemText.Data_PairOfInt16>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":{\"A\":1,\"B\":1}}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":{\"A\":-1,\"B\":-1}}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":{\"A\":2147483647,\"B\":2147483647}}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":{\"A\":-2147483648,\"B\":-2147483648}}")]
        public void Roundtrip_PairOfInt32(ValueKind kind, string expectedBytes)
        {
            PairOfInt32 value = kind switch
            {
                ValueKind.DefVal => new PairOfInt32(default, default),
                ValueKind.PosOne => new PairOfInt32(1, 1),
                ValueKind.NegOne => new PairOfInt32(-1, -1),
                ValueKind.MaxVal => new PairOfInt32(Int32.MaxValue, Int32.MaxValue),
                ValueKind.MinVal => new PairOfInt32(Int32.MinValue, Int32.MinValue),
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<PairOfInt32, Models.JsonSystemText.Data_PairOfInt32>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        [Theory]
        [InlineData(ValueKind.DefVal, "{}")]
        [InlineData(ValueKind.PosOne, "{\"Value\":{\"A\":1,\"B\":1}}")]
        [InlineData(ValueKind.NegOne, "{\"Value\":{\"A\":-1,\"B\":-1}}")]
        [InlineData(ValueKind.MaxVal, "{\"Value\":{\"A\":9223372036854775807,\"B\":9223372036854775807}}")]
        [InlineData(ValueKind.MinVal, "{\"Value\":{\"A\":-9223372036854775808,\"B\":-9223372036854775808}}")]
        public void Roundtrip_PairOfInt64(ValueKind kind, string expectedBytes)
        {
            PairOfInt64 value = kind switch
            {
                ValueKind.DefVal => new PairOfInt64(default, default),
                ValueKind.PosOne => new PairOfInt64(1, 1),
                ValueKind.NegOne => new PairOfInt64(-1, -1),
                ValueKind.MaxVal => new PairOfInt64(Int64.MaxValue, Int64.MaxValue),
                ValueKind.MinVal => new PairOfInt64(Int64.MinValue, Int64.MinValue),
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            Roundtrip2<PairOfInt64, Models.JsonSystemText.Data_PairOfInt64>(value, expectedBytes, (m, v) => { m.Value = v; }, (m) => m.Value);
        }

        // todo int128, uint128, binary

    }
}