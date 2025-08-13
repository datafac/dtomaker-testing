using System;
using DataFac.Memory;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Runtime;

namespace DTOMakerV10.Models
{
    [Entity][Id(1)][Layout(LayoutMethod.Linear)]
    public interface IData_SByte : IEntityBase { [Member(1)] sbyte Value { get; } }

    [Entity][Id(2)][Layout(LayoutMethod.Linear)]
    public interface IData_Byte : IEntityBase { [Member(1)] byte Value { get; } }

    [Entity][Id(3)][Layout(LayoutMethod.Linear)]
    public interface IData_Int16 : IEntityBase { [Member(1)] short Value { get; } }

    [Entity][Id(4)][Layout(LayoutMethod.Linear)]
    public interface IData_UInt16 : IEntityBase { [Member(1)] ushort Value { get; } }

    [Entity][Id(5)][Layout(LayoutMethod.Linear)]
    public interface IData_Int32 : IEntityBase { [Member(1)] int Value { get; } }

    [Entity][Id(6)][Layout(LayoutMethod.Linear)]
    public interface IData_UInt32 : IEntityBase { [Member(1)] uint Value { get; } }

    [Entity][Id(7)][Layout(LayoutMethod.Linear)]
    public interface IData_Int64 : IEntityBase { [Member(1)] long Value { get; } }

    [Entity][Id(8)][Layout(LayoutMethod.Linear)]
    public interface IData_UInt64 : IEntityBase { [Member(1)] ulong Value { get; } }

    [Entity][Id(9)][Layout(LayoutMethod.Linear)]
    public interface IData_Boolean : IEntityBase { [Member(1)] bool Value { get; } }

    [Entity][Id(10)][Layout(LayoutMethod.Linear)]
    public interface IData_Char : IEntityBase { [Member(1)] char Value { get; } }

    [Entity][Id(11)][Layout(LayoutMethod.Linear)]
    public interface IData_Single : IEntityBase { [Member(1)] float Value { get; } }

    [Entity][Id(12)][Layout(LayoutMethod.Linear)]
    public interface IData_Double : IEntityBase { [Member(1)] double Value { get; } }

    [Entity][Id(13)][Layout(LayoutMethod.Linear)]
    public interface IData_Decimal : IEntityBase { [Member(1)] decimal Value { get; } }

#if NET7_0_OR_GREATER
    [Entity][Id(14)][Layout(LayoutMethod.Linear)]
    public interface IData_Int128 : IEntityBase { [Member(1)] Int128 Value { get; } }

    [Entity][Id(15)][Layout(LayoutMethod.Linear)]
    public interface IData_UInt128 : IEntityBase { [Member(1)] UInt128 Value { get; } }
#endif

    [Entity][Id(16)][Layout(LayoutMethod.Linear)]
    public interface IData_Guid : IEntityBase { [Member(1)] Guid Value { get; } }

    [Entity][Id(17)][Layout(LayoutMethod.Linear)]
    public interface IData_String : IEntityBase { [Member(1)] string Value { get; } }

    [Entity][Id(18)][Layout(LayoutMethod.Linear)]
    public interface IData_Octets : IEntityBase { [Member(1)] Octets Value { get; set; } }

#if NET5_0_OR_GREATER
    [Entity][Id(21)][Layout(LayoutMethod.Linear)]
    public interface IData_Half : IEntityBase { [Member(1)] Half Value { get; } }
#endif

    [Entity][Id(22)][Layout(LayoutMethod.Linear)]
    public interface IData_PairOfInt16 : IEntityBase { [Member(1)] PairOfInt16 Value { get; } }

    [Entity][Id(23)][Layout(LayoutMethod.Linear)]
    public interface IData_PairOfInt32 : IEntityBase { [Member(1)] PairOfInt32 Value { get; } }

    [Entity][Id(24)][Layout(LayoutMethod.Linear)]
    public interface IData_PairOfInt64 : IEntityBase { [Member(1)] PairOfInt64 Value { get; } }

}
