using DataFac.Storage;
using DTOMaker.Runtime;
using Newtonsoft.Json.Linq;
using Sandbox.Generics.Models;
using Shouldly;
using System;
using Xunit;

namespace Sandbox.Generics.Tests
{
    public class GenericsTests_Trees
    {
        [Theory]
        [InlineData(ImplKind.MessagePack, "abc")]
        [InlineData(ImplKind.MemBlocks, "abc")]
        [InlineData(ImplKind.MemBlocks, "acb")]
        [InlineData(ImplKind.MemBlocks, "bac")]
        [InlineData(ImplKind.MemBlocks, "bca")]
        [InlineData(ImplKind.MemBlocks, "cab")]
        [InlineData(ImplKind.MemBlocks, "cba")]
        public void AddValue(ImplKind impl, string order)
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            Func<ITree<string, long>> nodeFactory = impl switch
            {
                ImplKind.MessagePack => () => new Sandbox.Generics.Models.MessagePack.MyTree(),
                ImplKind.MemBlocks => () => new Sandbox.Generics.Models.MemBlocks.MyTree(),
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
            //todo node.Depth.ShouldBe(expectedDepth);

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
