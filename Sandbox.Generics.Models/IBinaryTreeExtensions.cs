using DTOMaker.Runtime;
using System;
using System.Collections.Generic;

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

        public static IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs<TKey, TValue>(this IBinaryTree<TKey, TValue>? tree)
            where TKey : notnull, IComparable<TKey>
        {
            // todo sort by descending order if needed
            if (tree is null || !tree.HasValue) yield break;
            foreach (var kvp in tree.Left.GetKeyValuePairs()) yield return kvp;
            yield return new KeyValuePair<TKey, TValue>(tree.Key, tree.Value);
            foreach (var kvp in tree.Right.GetKeyValuePairs()) yield return kvp;
        }

        private static void TrySetCount<TKey, TValue>(this IBinaryTree<TKey, TValue>? tree)
        {
            if (tree is null) return;
            int newCount = 0;
            if (tree.HasValue) newCount = 1 + (tree.Left?.Count ?? 0) + (tree.Right?.Count ?? 0);
            if (tree.Count == newCount) return;
            tree.Count = newCount;
        }

        private static void TrySetDepth<TKey, TValue>(this IBinaryTree<TKey, TValue>? tree)
        {
            if (tree is null) return;
            byte newDepth = 0;
            if (tree.HasValue) newDepth = (byte)(1 + Math.Max(tree.Left?.Depth ?? 0, tree.Right?.Depth ?? 0));
            if (tree.Depth == newDepth) return;
            tree.Depth = newDepth;
        }

        private static IBinaryTree<TKey, TValue> Init<TKey, TValue>(this IBinaryTree<TKey, TValue> node, TKey key, TValue value)
            where TKey : notnull, IComparable<TKey>
        {
            node.HasValue = true;
            node.Key = key;
            node.Value = value;
            node.Count = 1;
            node.Depth = 1;
            node.Left = null;
            node.Right = null;
            return node;
        }

        public static IBinaryTree<TKey, TValue> AddOrUpdate<TKey, TValue>(this IBinaryTree<TKey, TValue> tree, TKey key, TValue value,
            Func<IBinaryTree<TKey, TValue>> newNodeFn)
            where TKey : notnull, IComparable<TKey>
        {
            IBinaryTree<TKey, TValue> result = tree.IsFrozen
                ? tree.PartCopy() as IBinaryTree<TKey, TValue> ?? throw new InvalidOperationException("Failed to create unfrozen copy.")
                : tree;

            if (!result.HasValue)
            {
                return result.Init(key, value);
            }

            int comparison = result.Key.CompareTo(key);
            if (comparison == 0)
            {
                // update value only
                result.Value = value;
                return result;
            }
            else if (comparison > 0)
            {
                // go left
                var left = result.Left;
                result.Left = left is null 
                    ? newNodeFn().Init(key, value) 
                    : left.AddOrUpdate(key, value, newNodeFn);
            }
            else
            {
                // go right
                var right = result.Right;
                result.Right = right is null
                    ? newNodeFn().Init(key, value)
                    : right.AddOrUpdate(key, value, newNodeFn);
            }

            // rebalance if needed
            bool rotated = false;
            // doco: https://en.wikipedia.org/wiki/Tree_rotation
            if (result.Left is not null && (result.Left.Depth - (result.Right?.Depth ?? 0)) > 1)
            {
                // left-heavy, perform right rotation
                // todo special case
                var newRoot = result.Left;
                result.Left = newRoot.Right;
                // unfreeze newRoot if needed
                newRoot = newRoot.Unfrozen();
                newRoot.Right = result;
                result = newRoot;
                rotated = true;
            }
            else if (result.Right is not null && (result.Right.Depth - (result.Left?.Depth ?? 0)) > 1)
            {
                // right-heavy, perform left rotation
                var newRoot = result.Right;
                result.Right = newRoot.Left;
                // unfreeze newRoot if needed
                newRoot = newRoot.Unfrozen();
                newRoot.Left = result;
                result = newRoot;
                rotated = true;
            }

            if (rotated)
            {
                // recalc count/depth for children
                if (result.Left is not null)
                {
                    result.Left.TrySetCount();
                    result.Left.TrySetDepth();
                }
                if (result.Right is not null)
                {
                    result.Right.TrySetCount();
                    result.Right.TrySetDepth();
                }
            }

            result.TrySetCount();
            result.TrySetDepth();
            return result;
        }
    }
}
