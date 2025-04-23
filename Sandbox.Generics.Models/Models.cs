using DTOMaker.Models;

namespace Sandbox.Generics.Models
{
    [Entity]
    [Id(100)]
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
    public interface IMyTree : ITree<string, long>
    {

    }
}
