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
        [Theory]
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

            Func<ITree<string, long>> nodeFactory = impl switch
            {
                ImplKind.MessagePack => () => new Sandbox.Generics.Models.MessagePack.MyTree(),
                ImplKind.MemBlocks => () => new Sandbox.Generics.Models.MemBlocks.MyTree(),
                ImplKind.JsonNewtonSoft => () => new Sandbox.Generics.Models.JsonNewtonSoft.MyTree(),
                ImplKind.CSPoco => () => new Sandbox.Generics.Models.CSPoco.MyTree(),
                _ => throw new NotSupportedException($"Unsupported implementation kind: {impl}"),
            };

            var tree = nodeFactory();

            // add nodes in order
            foreach (char ch in order)
            {
                long value = ch switch
                {
                    'a' => 1L,
                    'b' => -2L,
                    'c' => 3L,
                    _ => throw new ArgumentException($"Unexpected character: {ch}"),
                };
                tree = tree.AddOrUpdate(new string(ch, 1), value, nodeFactory);
            }

            // update
            tree = tree.AddOrUpdate("b", 2L, nodeFactory);

            if (tree is IPackable packable) packable.Pack(dataStore);
            if (tree is IFreezable freezable) freezable.Freeze();

            tree.Count.ShouldBe(3);
            tree.Depth.ShouldBe(expectedDepth);

            var node = tree.Get("a");
            node.ShouldNotBeNull();
            node.Key.ShouldBe("a");
            node.Value.ShouldBe(1L);

            node = tree.Get("b");
            node.ShouldNotBeNull();
            node.Key.ShouldBe("b");
            node.Value.ShouldBe(2L);

            node = tree.Get("c");
            node.ShouldNotBeNull();
            node.Key.ShouldBe("c");
            node.Value.ShouldBe(3L);

            node = tree.Get("d");
            node.ShouldBeNull();

        }
    }
}
