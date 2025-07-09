using DataFac.Storage;
using DTOMaker.Runtime;
using Sandbox.Generics.Models;
using Shouldly;
using System;
using Xunit;

namespace Sandbox.Generics.Tests
{
    public class GenericsTests_Trees
    {
        private static Func<IBinaryTree<string, long>> GetNodeFactory(ImplKind kind)
        {
            return kind switch
            {
                ImplKind.MessagePack => () => new Sandbox.Generics.Models.MessagePack.MyTree(),
                ImplKind.MemBlocks => () => new Sandbox.Generics.Models.MemBlocks.MyTree(),
                ImplKind.JsonNewtonSoft => () => new Sandbox.Generics.Models.JsonNewtonSoft.MyTree(),
                ImplKind.CSPoco => () => new Sandbox.Generics.Models.CSPoco.MyTree(),
                _ => throw new NotSupportedException($"Unsupported implementation kind: {kind}"),
            };
        }

        [Theory]
        [InlineData(ImplKind.CSPoco, "a", 1)]
        [InlineData(ImplKind.CSPoco, "abc", 3)]
        [InlineData(ImplKind.JsonNewtonSoft, "abc", 3)]
        [InlineData(ImplKind.MessagePack, "abc", 3)]
        [InlineData(ImplKind.MemBlocks, "abc", 3)]
        [InlineData(ImplKind.MemBlocks, "acb", 3)]
        [InlineData(ImplKind.MemBlocks, "bac", 2)]
        [InlineData(ImplKind.MemBlocks, "bca", 2)]
        [InlineData(ImplKind.MemBlocks, "cab", 3)]
        [InlineData(ImplKind.MemBlocks, "cba", 3)]
        public void AddValue(ImplKind impl, string order, int expectedDepth)
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            Func<IBinaryTree<string, long>> nodeFactory = GetNodeFactory(impl);

            var tree = nodeFactory();
            tree.Count.ShouldBe(0);
            tree.Depth.ShouldBe(0);

            // add nodes in order
            int count = 0;
            foreach (char ch in order)
            {
                long value = (Char.IsLetter(ch) && Char.IsLower(ch)) ? (ch - 'a') + 1 : throw new ArgumentException($"Unexpected character: {ch}");
                tree = tree.AddOrUpdate(new string(ch, 1), value, nodeFactory);
                count++;
            }

            if (tree is IPackable packable) packable.Pack(dataStore);
            if (tree is IFreezable freezable) freezable.Freeze();

            tree.Count.ShouldBe(count);
            tree.Depth.ShouldBe(expectedDepth);

            var node = tree.Get("a");
            node.ShouldNotBeNull();
            node.Key.ShouldBe("a");
            node.Value.ShouldBe(1L);

            node = tree.Get("z");
            node.ShouldBeNull();
        }

        [Theory]
        [InlineData(ImplKind.CSPoco, "a", 1, 1)]
        [InlineData(ImplKind.CSPoco, "bac", 2, 2)]
        [InlineData(ImplKind.CSPoco, "dbacfeg", 3, 3)]
        //todo [InlineData(ImplKind.CSPoco, "abcdefg", 7, 3)]
        public void AddValueAndRebalance(ImplKind impl, string order, int unbalancedDepth, int balancedDepth)
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            Func<IBinaryTree<string, long>> nodeFactory = GetNodeFactory(impl);

            var tree = nodeFactory();
            tree.Count.ShouldBe(0);
            tree.Depth.ShouldBe(0);

            // add nodes in order
            int count = 0;
            foreach (char ch in order)
            {
                long value = (Char.IsLetter(ch) && Char.IsLower(ch)) ? (ch - 'a') + 1 : throw new ArgumentException($"Unexpected character: {ch}");
                tree = tree.AddOrUpdate(new string(ch, 1), value, nodeFactory);
                count++;
            }

            if (tree is IPackable packable) packable.Pack(dataStore);
            if (tree is IFreezable freezable) freezable.Freeze();

            tree.Count.ShouldBe(count);
            tree.Depth.ShouldBe(unbalancedDepth);

            // todo re-balance if needed
            //tree = tree.Rebalance();
            tree.Depth.ShouldBe(balancedDepth);

        }

    }
}
