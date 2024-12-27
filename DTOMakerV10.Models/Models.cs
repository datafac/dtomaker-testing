using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;
using System;

namespace DTOMakerV10.Models
{
    [Entity][EntityKey(1)][Layout(LayoutMethod.SequentialV1)] public interface IData_Int16 { [Member(1)] Int16 Value { get; set; } }
    [Entity][EntityKey(2)][Layout(LayoutMethod.SequentialV1)] public interface IData_Int32 { [Member(1)] Int32 Value { get; set; } }
    [Entity][EntityKey(3)][Layout(LayoutMethod.SequentialV1)] public interface IData_Int64 { [Member(1)] Int64 Value { get; set; } }
}