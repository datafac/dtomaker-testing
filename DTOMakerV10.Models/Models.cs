using System;
using DataFac.Memory;
using DTOMaker.Models;
using DTOMaker.Runtime;

namespace DTOMakerV10.Models
{
    [Entity(1, LayoutMethod.Linear)]
    public interface IData_SByte : IEntityBase { [Member(1)] sbyte Value { get; } }

    [Entity(2, LayoutMethod.Linear)]
    public interface IData_Byte : IEntityBase { [Member(1)] byte Value { get; } }

    [Entity(3, LayoutMethod.Linear)]
    public interface IData_Int16 : IEntityBase { [Member(1)] short Value { get; } }

    [Entity(4, LayoutMethod.Linear)]
    public interface IData_UInt16 : IEntityBase { [Member(1)] ushort Value { get; } }

    [Entity(5, LayoutMethod.Linear)]
    public interface IData_Int32 : IEntityBase { [Member(1)] int Value { get; } }

    [Entity(6, LayoutMethod.Linear)]
    public interface IData_UInt32 : IEntityBase { [Member(1)] uint Value { get; } }

    [Entity(7, LayoutMethod.Linear)]
    public interface IData_Int64 : IEntityBase { [Member(1)] long Value { get; } }

    [Entity(8, LayoutMethod.Linear)]
    public interface IData_UInt64 : IEntityBase { [Member(1)] ulong Value { get; } }

    [Entity(9, LayoutMethod.Linear)]
    public interface IData_Boolean : IEntityBase { [Member(1)] bool Value { get; } }

    [Entity(10, LayoutMethod.Linear)]
    public interface IData_Char : IEntityBase { [Member(1)] char Value { get; } }

    [Entity(11, LayoutMethod.Linear)]
    public interface IData_Single : IEntityBase { [Member(1)] float Value { get; } }

    [Entity(12, LayoutMethod.Linear)]
    public interface IData_Double : IEntityBase { [Member(1)] double Value { get; } }

    [Entity(13, LayoutMethod.Linear)]
    public interface IData_Decimal : IEntityBase { [Member(1)] decimal Value { get; } }

#if NET7_0_OR_GREATER
    [Entity(14, LayoutMethod.Linear)]
    public interface IData_Int128 : IEntityBase { [Member(1)] Int128 Value { get; } }

    [Entity(15, LayoutMethod.Linear)]
    public interface IData_UInt128 : IEntityBase { [Member(1)] UInt128 Value { get; } }
#endif

    [Entity(16, LayoutMethod.Linear)]
    public interface IData_Guid : IEntityBase { [Member(1)] Guid Value { get; } }

    [Entity(17, LayoutMethod.Linear)]
    public interface IData_String : IEntityBase { [Member(1)] string Value { get; } }

    [Entity(18, LayoutMethod.Linear)]
    public interface IData_Octets : IEntityBase { [Member(1)] Octets Value { get; set; } }

#if NET5_0_OR_GREATER
    [Entity(21, LayoutMethod.Linear)]
    public interface IData_Half : IEntityBase { [Member(1)] Half Value { get; } }
#endif

    [Entity(22, LayoutMethod.Linear)]
    public interface IData_PairOfInt16 : IEntityBase { [Member(1)] PairOfInt16 Value { get; } }

    [Entity(23, LayoutMethod.Linear)]
    public interface IData_PairOfInt32 : IEntityBase { [Member(1)] PairOfInt32 Value { get; } }

    [Entity(24, LayoutMethod.Linear)]
    public interface IData_PairOfInt64 : IEntityBase { [Member(1)] PairOfInt64 Value { get; } }

}
