using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace Sandbox.Generics.Models
{
    [Entity]
    [Id(100)]
    [Layout(LayoutMethod.Linear)]
    public interface ITree<TKey, TValue>
    {
        [Member(1)] int Count { get; }
        [Member(2)] TKey Key { get; }
        [Member(3)] TValue Value { get; }
        [Member(4)] ITree<TKey, TValue>? Left { get; }
        [Member(5)] ITree<TKey, TValue>? Right { get; }
    }

    [Entity]
    [Id(1)]
    [Layout(LayoutMethod.Linear)]
    public interface IMyTree : ITree<string, long>
    {

    }
}
