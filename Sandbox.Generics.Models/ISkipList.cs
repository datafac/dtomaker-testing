using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace Sandbox.Generics.Models
{
    [Entity]
    [Id(101)]
    [Layout(LayoutMethod.Linear)]
    public interface ISkipListBase<T>
    {
        [Member(1)] long Total { get; set; }
    }

    [Entity]
    [Id(102)]
    [Layout(LayoutMethod.Linear)]
    public interface ISkipListNode<T> : ISkipListBase<T>
    {
        [Member(1)] ISkipListBase<T>? Node0 { get; set; }
        [Member(2)] ISkipListBase<T>? Node1 { get; set; }
        [Member(3)] ISkipListBase<T>? Node2 { get; set; }
        [Member(4)] ISkipListBase<T>? Node3 { get; set; }
        [Member(5)] ISkipListBase<T>? Node4 { get; set; }
        [Member(6)] ISkipListBase<T>? Node5 { get; set; }
        [Member(7)] ISkipListBase<T>? Node6 { get; set; }
        [Member(8)] ISkipListBase<T>? Node7 { get; set; }
        [Member(9)] long Total0 { get; set; }
        [Member(10)] long Total1 { get; set; }
        [Member(11)] long Total2 { get; set; }
        [Member(12)] long Total3 { get; set; }
        [Member(13)] long Total4 { get; set; }
        [Member(14)] long Total5 { get; set; }
        [Member(15)] long Total6 { get; set; }
        [Member(16)] long Total7 { get; set; }
    }

    [Entity]
    [Id(103)]
    [Layout(LayoutMethod.Linear)]
    public interface ISkipListLeaf<T> : ISkipListBase<T>
    {
        [Member(1)] T? Slot0 { get; set; }
        [Member(2)] T? Slot1 { get; set; }
        [Member(3)] T? Slot2 { get; set; }
        [Member(4)] T? Slot3 { get; set; }
        [Member(5)] T? Slot4 { get; set; }
        [Member(6)] T? Slot5 { get; set; }
        [Member(7)] T? Slot6 { get; set; }
        [Member(8)] T? Slot7 { get; set; }
    }

    [Entity][Id(903)][Layout(LayoutMethod.Linear)]
    public interface IMySkipListLeaf : ISkipListLeaf<string> { }

    [Entity][Id(902)][Layout(LayoutMethod.Linear)]
    public interface IMySkipListNode : ISkipListNode<string> { }
}