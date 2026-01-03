using DTOMaker.Models;
using DTOMaker.Runtime;

namespace Sandbox.Generics.Models
{
    public interface IBinaryTree<TKey, TValue> : IEntityBase
    {
        bool HasValue { get; set; }
        byte Depth { get; set; }
        int Count { get; set; }
        TKey Key { get; set; }
        TValue Value { get; set; }
        IBinaryTree<TKey, TValue>? Left { get; set; }
        IBinaryTree<TKey, TValue>? Right { get; set; }
    }
}