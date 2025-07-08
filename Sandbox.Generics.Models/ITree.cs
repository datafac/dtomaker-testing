using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace Sandbox.Generics.Models
{
    [Entity]
    [Id(100)]
    [Layout(LayoutMethod.Linear)]
    public interface ITree<TKey, TValue>
    {
        [Member(1)] int Count { get; set; }
        [Member(2)] int Depth { get; set; }
        [Member(3)] TKey Key { get; set; }
        [Member(4)] TValue Value { get; set; }
        [Member(5)] ITree<TKey, TValue>? Left { get; set; }
        [Member(6)] ITree<TKey, TValue>? Right { get; set; }
    }

}
