using DTOMaker.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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

        public static string DebugString<TKey, TValue>(this IBinaryTree<TKey, TValue>? tree)
        {
            if (tree is null || tree.Count == 0) return "";
            StringBuilder sb = new StringBuilder();
            sb.Append(tree.Key);
            sb.Append('=');
            sb.Append(tree.Value);
            if (tree.Left is not null || tree.Right is not null)
            {
                sb.Append('[');
                if (tree.Left is not null) sb.Append(tree.Left.DebugString());
                sb.Append('|');
                if (tree.Right is not null) sb.Append(tree.Right.DebugString());
                sb.Append(']');
            }
            return sb.ToString();
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

        public static IBinaryTree<TKey, TValue> AddOrUpdate<TKey, TValue>(this IBinaryTree<TKey, TValue> tree, TKey key, TValue value,
            Func<IBinaryTree<TKey, TValue>> nodeFactory, bool allowDuplicates = false)
            where TKey : notnull, IComparable<TKey>
        {
            if (allowDuplicates) // todo: Implement duplicates handling
                throw new NotSupportedException("Allowing duplicates is not supported in this implementation.");

            IBinaryTree<TKey, TValue> result = tree is IFreezable freezable && freezable.IsFrozen
                ? freezable.PartCopy() as IBinaryTree<TKey, TValue> ?? throw new InvalidOperationException("Failed to create unfrozen copy.")
                : tree;

            if (!result.HasValue)
            {
                // empty
                result.HasValue = true;
                result.Key = key;
                result.Value = value;
                result.Count = 1;
                result.Depth = 1;
                return result;
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
                if (result.Left is null) result.Left = nodeFactory();
                result.Left = result.Left.AddOrUpdate(key, value, nodeFactory, allowDuplicates);
            }
            else
            {
                // go right
                if (result.Right is null) result.Right = nodeFactory();
                result.Right = result.Right.AddOrUpdate(key, value, nodeFactory, allowDuplicates);
            }

            // rebalance if needed
            bool rebalanced = false;
            // doco: https://en.wikipedia.org/wiki/Tree_rotation
            if (result.Left is not null && (result.Left.Depth - (result.Right?.Depth ?? 0)) > 1)
            {
                // left-heavy, perform right rotation
                // todo special case
                var newRoot = result.Left;
                result.Left = newRoot.Right;
                newRoot.Right = result;
                result = newRoot;
                rebalanced = true;
            }
            else if (result.Right is not null && (result.Right.Depth - (result.Left?.Depth ?? 0)) > 1)
            {
                // right-heavy, perform left rotation
                var newRoot = result.Right;
                result.Right = newRoot.Left;
                newRoot.Left = result;
                result = newRoot;
                rebalanced = true;
            }

            if (rebalanced)
            {
                // recalc count/depth for children
                if (result.Left is not null)
                {
                    result.Left.Count = 1 + (result.Left.Left?.Count ?? 0) + (result.Left.Right?.Count ?? 0);
                    result.Left.Depth = 1 + Math.Max(result.Left.Left?.Depth ?? 0, result.Left.Right?.Depth ?? 0);
                }
                if (result.Right is not null)
                {
                    result.Right.Count = 1 + (result.Right.Left?.Count ?? 0) + (result.Right.Right?.Count ?? 0);
                    result.Right.Depth = 1 + Math.Max(result.Right.Left?.Depth ?? 0, result.Right.Right?.Depth ?? 0);
                }
            }

            result.Count = 1 + (result.Left?.Count ?? 0) + (result.Right?.Count ?? 0);
            result.Depth = 1 + Math.Max(result.Left?.Depth ?? 0, result.Right?.Depth ?? 0);
            return result;
        }
    }
}
