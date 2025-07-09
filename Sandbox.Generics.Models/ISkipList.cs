using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace Sandbox.Generics.Models
{
    [Entity]
    [Id(101)]
    [Layout(LayoutMethod.Linear)]
    public interface ISkipList<T>
    {
        [Member(1)] long Total { get; set; }
    }

    [Entity]
    [Id(102)]
    [Layout(LayoutMethod.Linear)]
    public interface ISkipListNode<T> : ISkipList<T>
    {
        [Member(1)] ISkipList<T>? Node0 { get; set; }
        [Member(2)] ISkipList<T>? Node1 { get; set; }
        [Member(3)] ISkipList<T>? Node2 { get; set; }
        [Member(4)] ISkipList<T>? Node3 { get; set; }
        [Member(5)] ISkipList<T>? Node4 { get; set; }
        [Member(6)] ISkipList<T>? Node5 { get; set; }
        [Member(7)] ISkipList<T>? Node6 { get; set; }
        [Member(8)] ISkipList<T>? Node7 { get; set; }
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
    public interface ISkipListLeaf<T> : ISkipList<T>
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
}