using DTOMaker.Models;
using DTOMaker.Runtime;
using System;
namespace DTOMakerV10.Models3
{
    [Entity][Id(1)][Layout(LayoutMethod.Linear)]
    public interface INode : IEntityBase
    {
        [Member(1)][FixedLength(16)] String Key { get; }
    }

    [Entity][Id(2)][Layout(LayoutMethod.Linear)]
    public interface IStringNode : INode
    {
        [Member(1)][FixedLength(64)] String Value { get; }
    }

    [Entity][Id(3)][Layout(LayoutMethod.Linear)]
    public interface INumericNode : INode
    {
    }

    [Entity][Id(4)][Layout(LayoutMethod.Linear)]
    public interface IInt64Node : INumericNode
    {
        [Member(1)] Int64 Value { get; }
    }

    [Entity][Id(5)][Layout(LayoutMethod.Linear)]
    public interface IDoubleNode : INumericNode
    {
        [Member(1)] Double Value { get; }
    }

    [Entity][Id(6)][Layout(LayoutMethod.Linear)]
    public interface IBooleanNode : INode
    {
        [Member(1)] Boolean Value { get; }
    }

    [Entity][Id(10)][Layout(LayoutMethod.Linear)]
    public interface ITree : IEntityBase
    {
        [Member(1)] ITree? Left { get; set; }
        [Member(2)] ITree? Right { get; set; }
        [Member(3)] INode? Node { get; set; } // todo allow required entity members
        [Member(4)] int Size { get; set; }
    }
}
