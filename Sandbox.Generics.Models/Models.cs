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

    public static class TreeExtensions
    {
        public static ITree<TKey, TValue> AddValue<TKey, TValue>(this ITree<TKey, TValue> tree, TKey key, TValue value, 
            Func<ITree<TKey, TValue>> nodeFactory, bool allowDuplicates = false)
            where TKey : notnull, IComparable<TKey>
        {
            if (allowDuplicates) // todo: Implement duplicates handling
                throw new NotSupportedException("Allowing duplicates is not supported in this implementation.");

            ITree<TKey, TValue> unfrozen = tree is IFreezable freezable && freezable.IsFrozen
                ? freezable.PartCopy() as ITree<TKey, TValue> ?? throw new InvalidOperationException("Failed to create unfrozen copy.")
                : tree;

            if (unfrozen.Count == 0)
            {
                // empty
                unfrozen.Key = key;
                unfrozen.Value = value;
                unfrozen.Count = 1;
            }
            else
            {
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
                    unfrozen.Left = unfrozen.Left.AddValue(key, value, nodeFactory, allowDuplicates);
                }
                else
                {
                    // go right
                    if (unfrozen.Right is null) unfrozen.Right = nodeFactory();
                    unfrozen.Right = unfrozen.Right.AddValue(key, value, nodeFactory, allowDuplicates);
                }
            }
            unfrozen.Count = 1 + (unfrozen.Left?.Count ?? 0) + (unfrozen.Right?.Count ?? 0);
            return unfrozen;
        }
    }
}
