using DTOMaker.Runtime;
using System;

namespace Sandbox.Generics.Models
{
    public static class IBinaryTreeExtensions
    {
        public static IBinaryTree<TKey, TValue>? Get<TKey, TValue>(this IBinaryTree<TKey, TValue> tree, TKey key)
            where TKey : notnull, IComparable<TKey>
        {
            if (tree is null) return null;
            int comparison = tree.Key.CompareTo(key);
            if (comparison == 0)
            {
                return tree; // found
            }
            else if (comparison > 0)
            {
                // go left
                return tree.Left?.Get(key);
            }
            else
            {
                // go right
                return tree.Right?.Get(key);
            }
        }

        public static IBinaryTree<TKey, TValue> AddOrUpdate<TKey, TValue>(this IBinaryTree<TKey, TValue> tree, TKey key, TValue value,
            Func<IBinaryTree<TKey, TValue>> nodeFactory, bool allowDuplicates = false)
            where TKey : notnull, IComparable<TKey>
        {
            if (allowDuplicates) // todo: Implement duplicates handling
                throw new NotSupportedException("Allowing duplicates is not supported in this implementation.");

            IBinaryTree<TKey, TValue> unfrozen = tree is IFreezable freezable && freezable.IsFrozen
                ? freezable.PartCopy() as IBinaryTree<TKey, TValue> ?? throw new InvalidOperationException("Failed to create unfrozen copy.")
                : tree;

            if (unfrozen.Count == 0)
            {
                // empty
                unfrozen.Key = key;
                unfrozen.Value = value;
                unfrozen.Count = 1;
                unfrozen.Depth = 1;
                return unfrozen;
            }

            int comparison = unfrozen.Key.CompareTo(key);
            if (comparison == 0)
            {
                // update value
                // count unchanged
                unfrozen.Value = value;
                return unfrozen;
            }
            else if (comparison > 0)
            {
                // go left
                if (unfrozen.Left is null) unfrozen.Left = nodeFactory();
                unfrozen.Left = unfrozen.Left.AddOrUpdate(key, value, nodeFactory, allowDuplicates);
            }
            else
            {
                // go right
                if (unfrozen.Right is null) unfrozen.Right = nodeFactory();
                unfrozen.Right = unfrozen.Right.AddOrUpdate(key, value, nodeFactory, allowDuplicates);
            }
            unfrozen.Count = 1 + (unfrozen.Left?.Count ?? 0) + (unfrozen.Right?.Count ?? 0);
            unfrozen.Depth = 1 + Math.Max(unfrozen.Left?.Depth ?? 0, unfrozen.Right?.Depth ?? 0);
            return unfrozen;
        }
    }
}
