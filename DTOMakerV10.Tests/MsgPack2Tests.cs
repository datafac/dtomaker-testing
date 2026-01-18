using DataFac.Memory;
using DTOMaker.Models;
using DTOMaker.Runtime.MsgPack2;
using DTOMakerV10.Models;
using Shouldly;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace DTOMakerV10.Tests
{
    public class MsgPack2Tests
    {
        private string Roundtrip3<TValue, TMsg>(TValue value, Action<TMsg, TValue> setValueFunc, Func<TMsg, TValue> getValueFunc)
            where TMsg : class, IEntityBase, IEquatable<TMsg>, new()
        {
            var sendMsg = new TMsg();
            setValueFunc(sendMsg, value);
            sendMsg.Freeze();

            // act
            var buffer = sendMsg.SerializeToMessagePack<TMsg>();
            TMsg? recdMsg = buffer.DeserializeFromMessagePack<TMsg>();
            recdMsg.ShouldNotBeNull();
            recdMsg.Freeze();

            // assert
            // - value
            TValue copyValue = getValueFunc(recdMsg);
            copyValue.ShouldBe(value);

            // - equality
            recdMsg.ShouldNotBeNull();
            recdMsg.Equals(sendMsg).ShouldBeTrue();
            recdMsg.ShouldBe(sendMsg);
            recdMsg.GetHashCode().ShouldBe(sendMsg.GetHashCode());

            // return buffer for verification
            return string.Join("-", buffer.ToArray().Select(x => x.ToString("X2")));
        }

        private string Roundtrip_Boolean_Value(Boolean value) => Roundtrip3<Boolean, Models.MsgPack2.Data_Boolean>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Boolean_False() => await Verifier.Verify(Roundtrip_Boolean_Value(false));
        [Fact] public async Task Roundtrip_Boolean_True() => await Verifier.Verify(Roundtrip_Boolean_Value(true));

        private string Roundtrip_SByte_Value(SByte value) => Roundtrip3<SByte, Models.MsgPack2.Data_SByte>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_SByte_DefVal() => await Verifier.Verify(Roundtrip_SByte_Value(0));
        [Fact] public async Task Roundtrip_SByte_PosOne() => await Verifier.Verify(Roundtrip_SByte_Value(1));
        [Fact] public async Task Roundtrip_SByte_NegOne() => await Verifier.Verify(Roundtrip_SByte_Value(-1));
        [Fact] public async Task Roundtrip_SByte_MaxVal() => await Verifier.Verify(Roundtrip_SByte_Value(SByte.MaxValue));
        [Fact] public async Task Roundtrip_SByte_MinVal() => await Verifier.Verify(Roundtrip_SByte_Value(SByte.MinValue));

        private string Roundtrip_Int16_Value(Int16 value) => Roundtrip3<Int16, Models.MsgPack2.Data_Int16>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Int16_DefVal() => await Verifier.Verify(Roundtrip_Int16_Value(0));
        [Fact] public async Task Roundtrip_Int16_PosOne() => await Verifier.Verify(Roundtrip_Int16_Value(1));
        [Fact] public async Task Roundtrip_Int16_NegOne() => await Verifier.Verify(Roundtrip_Int16_Value(-1));
        [Fact] public async Task Roundtrip_Int16_MaxVal() => await Verifier.Verify(Roundtrip_Int16_Value(Int16.MaxValue));
        [Fact] public async Task Roundtrip_Int16_MinVal() => await Verifier.Verify(Roundtrip_Int16_Value(Int16.MinValue));

        private string Roundtrip_Int32_Value(Int32 value) => Roundtrip3<Int32, Models.MsgPack2.Data_Int32>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Int32_DefVal() => await Verifier.Verify(Roundtrip_Int32_Value(0));
        [Fact] public async Task Roundtrip_Int32_PosOne() => await Verifier.Verify(Roundtrip_Int32_Value(1));
        [Fact] public async Task Roundtrip_Int32_NegOne() => await Verifier.Verify(Roundtrip_Int32_Value(-1));
        [Fact] public async Task Roundtrip_Int32_MaxVal() => await Verifier.Verify(Roundtrip_Int32_Value(Int32.MaxValue));
        [Fact] public async Task Roundtrip_Int32_MinVal() => await Verifier.Verify(Roundtrip_Int32_Value(Int32.MinValue));

        private string Roundtrip_Int64_Value(Int64 value) => Roundtrip3<Int64, Models.MsgPack2.Data_Int64>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Int64_DefVal() => await Verifier.Verify(Roundtrip_Int64_Value(0));
        [Fact] public async Task Roundtrip_Int64_PosOne() => await Verifier.Verify(Roundtrip_Int64_Value(1));
        [Fact] public async Task Roundtrip_Int64_NegOne() => await Verifier.Verify(Roundtrip_Int64_Value(-1));
        [Fact] public async Task Roundtrip_Int64_MaxVal() => await Verifier.Verify(Roundtrip_Int64_Value(Int64.MaxValue));
        [Fact] public async Task Roundtrip_Int64_MinVal() => await Verifier.Verify(Roundtrip_Int64_Value(Int64.MinValue));

#if NET8_0_OR_GREATER
        private string Roundtrip_Int128_Value(Int128 value) => Roundtrip3<Int128, Models.MsgPack2.Data_Int128>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Int128_DefVal() => await Verifier.Verify(Roundtrip_Int128_Value(0));
        [Fact] public async Task Roundtrip_Int128_PosOne() => await Verifier.Verify(Roundtrip_Int128_Value(1));
        [Fact] public async Task Roundtrip_Int128_NegOne() => await Verifier.Verify(Roundtrip_Int128_Value(-1));
        [Fact] public async Task Roundtrip_Int128_MaxVal() => await Verifier.Verify(Roundtrip_Int128_Value(Int128.MaxValue));
        [Fact] public async Task Roundtrip_Int128_MinVal() => await Verifier.Verify(Roundtrip_Int128_Value(Int128.MinValue));
#endif

        private string Roundtrip_Byte_Value(Byte value) => Roundtrip3<Byte, Models.MsgPack2.Data_Byte>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Byte_DefVal() => await Verifier.Verify(Roundtrip_Byte_Value(0));
        [Fact] public async Task Roundtrip_Byte_PosOne() => await Verifier.Verify(Roundtrip_Byte_Value(1));
        [Fact] public async Task Roundtrip_Byte_MaxVal() => await Verifier.Verify(Roundtrip_Byte_Value(Byte.MaxValue));

        private string Roundtrip_UInt16_Value(UInt16 value) => Roundtrip3<UInt16, Models.MsgPack2.Data_UInt16>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_UInt16_DefVal() => await Verifier.Verify(Roundtrip_UInt16_Value(0));
        [Fact] public async Task Roundtrip_UInt16_PosOne() => await Verifier.Verify(Roundtrip_UInt16_Value(1));
        [Fact] public async Task Roundtrip_UInt16_MaxVal() => await Verifier.Verify(Roundtrip_UInt16_Value(UInt16.MaxValue));

        private string Roundtrip_UInt32_Value(UInt32 value) => Roundtrip3<UInt32, Models.MsgPack2.Data_UInt32>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_UInt32_DefVal() => await Verifier.Verify(Roundtrip_UInt32_Value(0));
        [Fact] public async Task Roundtrip_UInt32_PosOne() => await Verifier.Verify(Roundtrip_UInt32_Value(1));
        [Fact] public async Task Roundtrip_UInt32_MaxVal() => await Verifier.Verify(Roundtrip_UInt32_Value(UInt32.MaxValue));

        private string Roundtrip_UInt64_Value(UInt64 value) => Roundtrip3<UInt64, Models.MsgPack2.Data_UInt64>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_UInt64_DefVal() => await Verifier.Verify(Roundtrip_UInt64_Value(0));
        [Fact] public async Task Roundtrip_UInt64_PosOne() => await Verifier.Verify(Roundtrip_UInt64_Value(1));
        [Fact] public async Task Roundtrip_UInt64_MaxVal() => await Verifier.Verify(Roundtrip_UInt64_Value(UInt64.MaxValue));

#if NET8_0_OR_GREATER
        private string Roundtrip_UInt128_Value(UInt128 value) => Roundtrip3<UInt128, Models.MsgPack2.Data_UInt128>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_UInt128_DefVal() => await Verifier.Verify(Roundtrip_UInt128_Value(0));
        [Fact] public async Task Roundtrip_UInt128_PosOne() => await Verifier.Verify(Roundtrip_UInt128_Value(1));
        [Fact] public async Task Roundtrip_UInt128_MaxVal() => await Verifier.Verify(Roundtrip_UInt128_Value(UInt128.MaxValue));
#endif

        private static readonly Guid AnyRndGuid = Guid.Parse("1f4a6e09-8bce-4f76-9bc9-6b9c7f06c7ca");
        private string Roundtrip_Guid_Value(Guid value) => Roundtrip3<Guid, Models.MsgPack2.Data_Guid>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Guid_DefVal() => await Verifier.Verify(Roundtrip_Guid_Value(Guid.Empty));
        [Fact] public async Task Roundtrip_Guid_RndVal() => await Verifier.Verify(Roundtrip_Guid_Value(AnyRndGuid));

        private string Roundtrip_Char_Value(Char value) => Roundtrip3<Char, Models.MsgPack2.Data_Char>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Char_DefVal() => await Verifier.Verify(Roundtrip_Char_Value(default));
        [Fact] public async Task Roundtrip_Char_PosOne() => await Verifier.Verify(Roundtrip_Char_Value(' '));
        [Fact] public async Task Roundtrip_Char_MaxVal() => await Verifier.Verify(Roundtrip_Char_Value(Char.MaxValue));

        private string Roundtrip_Decimal_Value(Decimal value) => Roundtrip3<Decimal, Models.MsgPack2.Data_Decimal>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Decimal_DefVal() => await Verifier.Verify(Roundtrip_Decimal_Value(Decimal.Zero));
        [Fact] public async Task Roundtrip_Decimal_PosOne() => await Verifier.Verify(Roundtrip_Decimal_Value(Decimal.One));
        [Fact] public async Task Roundtrip_Decimal_NegOne() => await Verifier.Verify(Roundtrip_Decimal_Value(Decimal.MinusOne));
        [Fact] public async Task Roundtrip_Decimal_MaxVal() => await Verifier.Verify(Roundtrip_Decimal_Value(Decimal.MaxValue));
        [Fact] public async Task Roundtrip_Decimal_MinVal() => await Verifier.Verify(Roundtrip_Decimal_Value(Decimal.MinValue));

        private string Roundtrip_PairOfInt16_Value(PairOfInt16 value) => Roundtrip3<PairOfInt16, Models.MsgPack2.Data_PairOfInt16>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_PairOfInt16_DefVal() => await Verifier.Verify(Roundtrip_PairOfInt16_Value(default));
        [Fact] public async Task Roundtrip_PairOfInt16_PosNeg() => await Verifier.Verify(Roundtrip_PairOfInt16_Value(new PairOfInt16(1, -1)));
        [Fact] public async Task Roundtrip_PairOfInt16_MaxMin() => await Verifier.Verify(Roundtrip_PairOfInt16_Value(new PairOfInt16(Int16.MaxValue, Int16.MinValue)));

        private string Roundtrip_PairOfInt32_Value(PairOfInt32 value) => Roundtrip3<PairOfInt32, Models.MsgPack2.Data_PairOfInt32>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_PairOfInt32_DefVal() => await Verifier.Verify(Roundtrip_PairOfInt32_Value(default));
        [Fact] public async Task Roundtrip_PairOfInt32_PosNeg() => await Verifier.Verify(Roundtrip_PairOfInt32_Value(new PairOfInt32(1, -1)));
        [Fact] public async Task Roundtrip_PairOfInt32_MaxMin() => await Verifier.Verify(Roundtrip_PairOfInt32_Value(new PairOfInt32(Int32.MaxValue, Int32.MinValue)));

        private string Roundtrip_PairOfInt64_Value(PairOfInt64 value) => Roundtrip3<PairOfInt64, Models.MsgPack2.Data_PairOfInt64>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_PairOfInt64_DefVal() => await Verifier.Verify(Roundtrip_PairOfInt64_Value(default));
        [Fact] public async Task Roundtrip_PairOfInt64_PosNeg() => await Verifier.Verify(Roundtrip_PairOfInt64_Value(new PairOfInt64(1, -1)));
        [Fact] public async Task Roundtrip_PairOfInt64_MaxMin() => await Verifier.Verify(Roundtrip_PairOfInt64_Value(new PairOfInt64(Int64.MaxValue, Int64.MinValue)));

        private string Roundtrip_String_Value(String value) => Roundtrip3<String, Models.MsgPack2.Data_String>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_String_Empty() => await Verifier.Verify(Roundtrip_String_Value(string.Empty));
        [Fact] public async Task Roundtrip_String_RndVal() => await Verifier.Verify(Roundtrip_String_Value("abcdef"));

        private static readonly Octets AnyRndOctets = new Octets(Encoding.UTF8.GetBytes("abcdef"));
        private string Roundtrip_Octets_Value(Octets value) => Roundtrip3<Octets, Models.MsgPack2.Data_Octets>(value, (m, v) => { ((IData_Octets)m).Value = v; }, (m) => ((IData_Octets)m).Value);
        [Fact] public async Task Roundtrip_Octets_Empty() => await Verifier.Verify(Roundtrip_Octets_Value(Octets.Empty));
        [Fact] public async Task Roundtrip_Octets_RndVal() => await Verifier.Verify(Roundtrip_Octets_Value(AnyRndOctets));

#if NET8_0_OR_GREATER
        private string Roundtrip_Half_Value(Half value) => Roundtrip3<Half, Models.MsgPack2.Data_Half>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Half_DefVal() => await Verifier.Verify(Roundtrip_Half_Value(Half.Zero));
        [Fact] public async Task Roundtrip_Half_PosOne() => await Verifier.Verify(Roundtrip_Half_Value(Half.One));
        [Fact] public async Task Roundtrip_Half_NegOne() => await Verifier.Verify(Roundtrip_Half_Value(Half.NegativeOne));
        [Fact] public async Task Roundtrip_Half_MinInc() => await Verifier.Verify(Roundtrip_Half_Value(Half.Epsilon));
        [Fact] public async Task Roundtrip_Half_MaxVal() => await Verifier.Verify(Roundtrip_Half_Value(Half.MaxValue));
        [Fact] public async Task Roundtrip_Half_MinVal() => await Verifier.Verify(Roundtrip_Half_Value(Half.MinValue));
        [Fact] public async Task Roundtrip_Half_PosInf() => await Verifier.Verify(Roundtrip_Half_Value(Half.PositiveInfinity));
        [Fact] public async Task Roundtrip_Half_NegInf() => await Verifier.Verify(Roundtrip_Half_Value(Half.NegativeInfinity));
        //[Fact] public async Task Roundtrip_Half_NotNum() => await Verifier.Verify(Roundtrip_Half_Value(Half.NaN)); todo NaN equality check fails
        [Fact] public async Task Roundtrip_Half_ValE() => await Verifier.Verify(Roundtrip_Half_Value(Half.E));
        [Fact] public async Task Roundtrip_Half_ValPi() => await Verifier.Verify(Roundtrip_Half_Value(Half.Pi));
        [Fact] public async Task Roundtrip_Half_ValTau() => await Verifier.Verify(Roundtrip_Half_Value(Half.Tau));
#endif

        private string Roundtrip_Double_Value(Double value) => Roundtrip3<Double, Models.MsgPack2.Data_Double>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Double_DefVal() => await Verifier.Verify(Roundtrip_Double_Value(default));
        [Fact] public async Task Roundtrip_Double_PosOne() => await Verifier.Verify(Roundtrip_Double_Value(1D));
        [Fact] public async Task Roundtrip_Double_NegOne() => await Verifier.Verify(Roundtrip_Double_Value(-1D));
#if NET8_0_OR_GREATER
        [Fact] public async Task Roundtrip_Double_MinInc_Net80() => await Verifier.Verify(Roundtrip_Double_Value(Double.Epsilon));
#else
        [Fact] public async Task Roundtrip_Double_MinInc_Net48() => await Verifier.Verify(Roundtrip_Double_Value(Double.Epsilon));
#endif
        [Fact] public async Task Roundtrip_Double_MaxVal() => await Verifier.Verify(Roundtrip_Double_Value(Double.MaxValue));
        [Fact] public async Task Roundtrip_Double_MinVal() => await Verifier.Verify(Roundtrip_Double_Value(Double.MinValue));
        [Fact] public async Task Roundtrip_Double_PosInf() => await Verifier.Verify(Roundtrip_Double_Value(Double.PositiveInfinity));
        [Fact] public async Task Roundtrip_Double_NegInf() => await Verifier.Verify(Roundtrip_Double_Value(Double.NegativeInfinity));
        //[Fact] public async Task Roundtrip_Double_NotNum() => await Verifier.Verify(Roundtrip_Double_Value(Double.NaN)); // todo NaN equality check fails
#if NET8_0_OR_GREATER
        [Fact] public async Task Roundtrip_Double_ValE() => await Verifier.Verify(Roundtrip_Double_Value(Double.E));
        [Fact] public async Task Roundtrip_Double_ValPi() => await Verifier.Verify(Roundtrip_Double_Value(Double.Pi));
        [Fact] public async Task Roundtrip_Double_ValTau() => await Verifier.Verify(Roundtrip_Double_Value(Double.Tau));
#endif

        private string Roundtrip_Single_Value(Single value) => Roundtrip3<Single, Models.MsgPack2.Data_Single>(value, (m, v) => { m.Value = v; }, (m) => m.Value);
        [Fact] public async Task Roundtrip_Single_DefVal() => await Verifier.Verify(Roundtrip_Single_Value(default));
        [Fact] public async Task Roundtrip_Single_PosOne() => await Verifier.Verify(Roundtrip_Single_Value(1F));
        [Fact] public async Task Roundtrip_Single_NegOne() => await Verifier.Verify(Roundtrip_Single_Value(-1F));
#if NET8_0_OR_GREATER
        [Fact] public async Task Roundtrip_Single_MinInc_Net80() => await Verifier.Verify(Roundtrip_Single_Value(Single.Epsilon));
#else
        [Fact] public async Task Roundtrip_Single_MinInc_Net48() => await Verifier.Verify(Roundtrip_Single_Value(Single.Epsilon));
#endif
#if NET8_0_OR_GREATER
        [Fact] public async Task Roundtrip_Single_MaxVal_Net80() => await Verifier.Verify(Roundtrip_Single_Value(Single.MaxValue));
#else
        [Fact] public async Task Roundtrip_Single_MaxVal_Net48() => await Verifier.Verify(Roundtrip_Single_Value(Single.MaxValue));
#endif
#if NET8_0_OR_GREATER
        [Fact] public async Task Roundtrip_Single_MinVal_Net80() => await Verifier.Verify(Roundtrip_Single_Value(Single.MinValue));
#else
        [Fact] public async Task Roundtrip_Single_MinVal_Net48() => await Verifier.Verify(Roundtrip_Single_Value(Single.MinValue));
#endif
        [Fact] public async Task Roundtrip_Single_PosInf() => await Verifier.Verify(Roundtrip_Single_Value(Single.PositiveInfinity));
        [Fact] public async Task Roundtrip_Single_NegInf() => await Verifier.Verify(Roundtrip_Single_Value(Single.NegativeInfinity));
        //[Fact] public async Task Roundtrip_Single_NotNum() => await Verifier.Verify(Roundtrip_Single_Value(Single.NaN)); // todo NaN equality check fails
#if NET8_0_OR_GREATER
        [Fact] public async Task Roundtrip_Single_ValE() => await Verifier.Verify(Roundtrip_Single_Value(Single.E));
        [Fact] public async Task Roundtrip_Single_ValPi() => await Verifier.Verify(Roundtrip_Single_Value(Single.Pi));
        [Fact] public async Task Roundtrip_Single_ValTau() => await Verifier.Verify(Roundtrip_Single_Value(Single.Tau));
#endif
    }
}
