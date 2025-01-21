using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;
using System;

namespace DTOMakerV10.Models.Basics
{
    [Entity][EntityKey(1)][Layout(LayoutMethod.Linear)] public interface IData_SByte { [Member(1)] sbyte Value { get; set; } }
    [Entity][EntityKey(2)][Layout(LayoutMethod.Linear)] public interface IData_Byte { [Member(1)] byte Value { get; set; } }
    [Entity][EntityKey(3)][Layout(LayoutMethod.Linear)] public interface IData_Int16 { [Member(1)] short Value { get; set; } }
    [Entity][EntityKey(4)][Layout(LayoutMethod.Linear)] public interface IData_UInt16 { [Member(1)] ushort Value { get; set; } }
    [Entity][EntityKey(5)][Layout(LayoutMethod.Linear)] public interface IData_Int32 { [Member(1)] int Value { get; set; } }
    [Entity][EntityKey(6)][Layout(LayoutMethod.Linear)] public interface IData_UInt32 { [Member(1)] uint Value { get; set; } }
    [Entity][EntityKey(7)][Layout(LayoutMethod.Linear)] public interface IData_Int64 { [Member(1)] long Value { get; set; } }
    [Entity][EntityKey(8)][Layout(LayoutMethod.Linear)] public interface IData_UInt64 { [Member(1)] ulong Value { get; set; } }
    [Entity][EntityKey(9)][Layout(LayoutMethod.Linear)] public interface IData_Boolean { [Member(1)] bool Value { get; set; } }
}