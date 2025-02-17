using System;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;
namespace DTOMakerV10.Models3
{
    [Entity][EntityKey(1)][Layout(LayoutMethod.Linear)]
    [Id("b931ff5b-26b5-4118-9a99-49f036f2f386")]
    public interface INode
    {
        [Member(1)][StrLen(16)] String Key { get; set; }
    }

    [Entity][EntityKey(2)][Layout(LayoutMethod.Linear)]
    [Id("91596c25-51a1-40b1-aff8-f2f07e252669")]
    public interface IStringNode : INode
    {
        [Member(1)][StrLen(64)] String Value { get; set; }
    }

    [Entity][EntityKey(3)][Layout(LayoutMethod.Linear)]
    [Id("2ff258b1-79da-410b-895d-6493194e908b")]
    public interface INumericNode : INode
    {
    }

    [Entity][EntityKey(4)][Layout(LayoutMethod.Linear)]
    [Id("e4edc645-9b28-4970-8d1e-06d8a7ed94a5")]
    public interface IInt64Node : INumericNode
    {
        [Member(1)] Int64 Value { get; set; }
    }

    [Entity][EntityKey(5)][Layout(LayoutMethod.Linear)]
    [Id("10334a1c-fb06-4d63-bff9-53aba5ccc7ac")]
    public interface IDoubleNode : INumericNode
    {
        [Member(1)] Double Value { get; set; }
    }

    [Entity][EntityKey(6)][Layout(LayoutMethod.Linear)]
    [Id("7dac9920-aecf-4ec1-9a62-32e99539b62c")]
    public interface IBooleanNode : INode
    {
        [Member(1)] Boolean Value { get; set; }
    }

    [Entity][EntityKey(10)][Layout(LayoutMethod.Linear)]
    [Id("8555eef6-f9b4-451d-86c2-0f3c61e1261b")]
    public interface ITree
    {
        [Member(1)] ITree? Left { get; set; }
        [Member(2)] ITree? Right { get; set; }
        [Member(3)] INode? Node { get; set; } // todo allow required entity members
        [Member(4)] int Size { get; set; }
    }
}
