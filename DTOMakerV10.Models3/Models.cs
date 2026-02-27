using DTOMaker.Models;
using System;
namespace DTOMakerV10.Models3
{
    [Entity(1)]
    public interface INode : IEntityBase
    {
        [Member(1)] String Key { get; }
    }

    [Entity(2)]
    public interface IStringNode : INode
    {
        [Member(1)] String Value { get; }
    }

    [Entity(3)]
    public interface INumericNode : INode
    {
    }

    [Entity(4)]
    public interface IInt64Node : INumericNode
    {
        [Member(1)] Int64 Value { get; }
    }

    [Entity(5)]
    public interface IDoubleNode : INumericNode
    {
        [Member(1)] Double Value { get; }
    }

    [Entity(6)]
    public interface IBooleanNode : INode
    {
        [Member(1)] Boolean Value { get; }
    }

    [Entity(10)]
    public interface ITree : IEntityBase
    {
        [Member(1)] ITree? Left { get; set; }
        [Member(2)] ITree? Right { get; set; }
        [Member(3)] INode? Node { get; set; } // todo allow required entity members
        [Member(4)] int Size { get; set; }
    }
}
