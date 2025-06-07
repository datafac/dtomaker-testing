using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Runtime;
using System;

namespace Sandbox.Generics.Models
{
    [Entity]
    [Id(100)]
    [Layout(LayoutMethod.Linear)]
    public interface ITree<TKey, TValue>
    {
        [Member(1)] int Count { get; set; }
        [Member(2)] TKey Key { get; set; }
        [Member(3)] TValue Value { get; set; }
        [Member(4)] ITree<TKey, TValue>? Left { get; set; }
        [Member(5)] ITree<TKey, TValue>? Right { get; set; }
    }

    [Entity]
    [Id(1)]
    [Layout(LayoutMethod.Linear)]
    public interface IMyTree : ITree<string, long>
    {

    }
}
