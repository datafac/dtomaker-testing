using DataFac.Memory;
using DTOMaker.Models;
using DTOMaker.Models.BinaryTree;
using System;

namespace TestModels
{
    [Entity(1)]
    public interface IMyDTO : IEntityBase
    {
        [Member(1)] bool FBool01 { get; set; }
        [Member(2)] int  FSInt04 { get; set; }
        [Member(3)][Endian(false)] double Field02LE { get; set; }
        [Member(4)][Endian(true)] double Field03BE { get; set; }
        [Member(5)] Guid Field04 { get; set; }
        [Member(6)] string? Field05 { get; set; }
        [Member(7)] Octets? Field06 { get; set; }
        [Member(8)] PairOfInt16 Field07 { get; set; }
        [Member(9)] PairOfInt32 Field08 { get; set; }
        [Member(10)] PairOfInt64 Field09 { get; set; }

        //todo [Member(n)] DayOfWeek Field0n { get; set; }
    }

    [Entity(2)]
    public interface ICustom1 : IEntityBase
    {
        [Member(1, NativeType.Int32, typeof(DayOfWeekConverter))] DayOfWeek Field1 { get; set; }
    }

    [Entity(3)]
    public interface IShape : IEntityBase { }

    [Entity(4)]
    public interface ITriangle : IShape { }

    [Entity(5)]
    public interface IEquilateral : ITriangle
    {
        [Member(1)] double Length { get; }
    }

    [Entity(6)]
    public interface IRightTriangle : ITriangle
    {
        [Member(1)] double Length { get; }
        [Member(2)] double Height { get; }
    }

    [Entity(7)]
    public interface IQuadrilateral : IShape { }

    [Entity(8)]
    public interface ISquare : IQuadrilateral
    {
        [Member(1)] double Length { get; }
    }

    [Entity(9)]
    public interface IRectangle : IQuadrilateral
    {
        [Member(1)] double Length { get; }
        [Member(2)] double Height { get; }
    }

    [Entity(10)]
    public interface ITextTree : IEntityBase
    {
        [Member(1)] string Value { get; set; }
        [Member(2)] int Key { get; set; }
        [Member(3)] int Count { get; set; }
        [Member(4)] byte Depth { get; set; }
        [Member(5)] ITextTree? Left { get; set; }
        [Member(6)] ITextTree? Right { get; set; }
    }
}

namespace TestModels.JsonSystemText
{
    public partial class TextTree : IBinaryTree<int, string, TextTree> { }
}

namespace TestModels.JsonNewtonSoft
{
    public partial class TextTree : IBinaryTree<int, string, TextTree> { }
}

namespace TestModels.MsgPack2
{
    public partial class TextTree : IBinaryTree<int, string, TextTree> { }
}

namespace TestModels.MemBlocks
{
    public partial class TextTree : IBinaryTree<int, string, TextTree> { }
}
