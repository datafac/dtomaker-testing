using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace Sandbox.Generics.Models
{
    [Entity]
    [Id(100)]
    [Layout(LayoutMethod.Linear)]
    public interface IBinaryTree<TKey, TValue>
    {
        [Member(1)] int Count { get; set; }
        [Member(2)] int Depth { get; set; }
        [Member(3)] bool HasValue { get; set; }
        [Member(4)] TKey Key { get; set; }
        [Member(5)] TValue Value { get; set; }
        [Member(6)] IBinaryTree<TKey, TValue>? Left { get; set; }
        [Member(7)] IBinaryTree<TKey, TValue>? Right { get; set; }
    }
}
