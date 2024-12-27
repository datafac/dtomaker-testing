using DTOMaker.Models;
using DTOMaker.Models.MessagePack;
using System;

namespace DTOMakerV10.Models
{
    [Entity][EntityTag(1)] public interface IData_Int16 { [Member(1)] Int16 Value { get; set; } }
    [Entity][EntityTag(2)] public interface IData_Int32 { [Member(1)] Int32 Value { get; set; } }
}