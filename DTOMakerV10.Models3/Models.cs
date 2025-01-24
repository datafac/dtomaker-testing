using System;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;
namespace DTOMakerV10.Models3
{
    [Entity][EntityKey(1)][Layout(LayoutMethod.Linear)]
    public interface INode
    {
        [Member(1)] String Key { get; set; }
    }

    [Entity][EntityKey(2)][Layout(LayoutMethod.Linear)]
    public interface IStringNode : INode
    {
        [Member(1)] String Value { get; set; }
    }

    [Entity][EntityKey(3)][Layout(LayoutMethod.Linear)]
    public interface INumericNode : INode
    {
    }

    [Entity][EntityKey(4)][Layout(LayoutMethod.Linear)]
    public interface IInt64Node : INumericNode
    {
        [Member(1)] Int64 Value { get; set; }
    }

    [Entity][EntityKey(5)][Layout(LayoutMethod.Linear)]
    public interface IDoubleNode : INumericNode
    {
        [Member(1)] Double Value { get; set; }
    }

    [Entity][EntityKey(6)][Layout(LayoutMethod.Linear)]
    public interface IBooleanNode : INode
    {
        [Member(1)] Boolean Value { get; set; }
    }

    [Entity][EntityKey(10)][Layout(LayoutMethod.Linear)]
    public interface ITree
    {
        [Member(1)] ITree? Left { get; set; }
        [Member(2)] ITree? Right { get; set; }
        [Member(3)] INode? Node { get; set; }
        [Member(4)] int Size { get; set; }
    }
}
