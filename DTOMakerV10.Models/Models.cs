using System;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;

namespace DTOMakerV10.Models
{
    [Entity][Id(1)][Layout(LayoutMethod.Linear)]
    public interface IData_SByte { [Member(1)] sbyte Value { get; set; } }

    [Entity][Id(2)][Layout(LayoutMethod.Linear)]
    public interface IData_Byte { [Member(1)] byte Value { get; set; } }

    [Entity][Id(3)][Layout(LayoutMethod.Linear)]
    public interface IData_Int16 { [Member(1)] short Value { get; set; } }

    [Entity][Id(4)][Layout(LayoutMethod.Linear)]
    public interface IData_UInt16 { [Member(1)] ushort Value { get; set; } }

    [Entity][Id(5)][Layout(LayoutMethod.Linear)]
    public interface IData_Int32 { [Member(1)] int Value { get; set; } }

    [Entity][Id(6)][Layout(LayoutMethod.Linear)]
    public interface IData_UInt32 { [Member(1)] uint Value { get; set; } }

    [Entity][Id(7)][Layout(LayoutMethod.Linear)]
    public interface IData_Int64 { [Member(1)] long Value { get; set; } }

    [Entity][Id(8)][Layout(LayoutMethod.Linear)]
    public interface IData_UInt64 { [Member(1)] ulong Value { get; set; } }

    [Entity][Id(9)][Layout(LayoutMethod.Linear)]
    public interface IData_Boolean { [Member(1)] bool Value { get; set; } }

    [Entity][Id(10)][Layout(LayoutMethod.Linear)]
    public interface IData_Char { [Member(1)] char Value { get; set; } }

    [Entity][Id(11)][Layout(LayoutMethod.Linear)]
    public interface IData_Single { [Member(1)] float Value { get; set; } }

    [Entity][Id(12)][Layout(LayoutMethod.Linear)]
    public interface IData_Double { [Member(1)] double Value { get; set; } }

    [Entity][Id(13)][Layout(LayoutMethod.Linear)]
    public interface IData_Decimal { [Member(1)] decimal Value { get; set; } }

#if NET7_0_OR_GREATER
    [Entity][Id(14)][Layout(LayoutMethod.Linear)]
    public interface IData_Int128 { [Member(1)] Int128 Value { get; set; } }

    [Entity][Id(15)][Layout(LayoutMethod.Linear)]
    public interface IData_UInt128 { [Member(1)] UInt128 Value { get; set; } }
#endif

    [Entity][Id(16)][Layout(LayoutMethod.Linear)]
    public interface IData_Guid { [Member(1)] Guid Value { get; set; } }

#if NET5_0_OR_GREATER
    [Entity][Id(21)][Layout(LayoutMethod.Linear)]
    public interface IData_Half { [Member(1)] Half Value { get; set; } }
#endif


    // todo Guid, Int128, UInt128
    // todo String
}