using DataFac.Storage;
using DTOMaker.Runtime;
using Sandbox.Generics.Models;
using Shouldly;
using System;
using System.Linq;
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
        [InlineData(ImplKind.CSPoco, "b", 1)]
        [InlineData(ImplKind.CSPoco, "ba", 2)]
        [InlineData(ImplKind.CSPoco, "bc", 2)]
        [InlineData(ImplKind.CSPoco, "abc", 2)]
        [InlineData(ImplKind.CSPoco, "acb", 3)]
        [InlineData(ImplKind.CSPoco, "bac", 2)]
        [InlineData(ImplKind.CSPoco, "bca", 2)]
        [InlineData(ImplKind.CSPoco, "cba", 2)]
        [InlineData(ImplKind.CSPoco, "cab", 3)]
        [InlineData(ImplKind.CSPoco, "dbacfeg", 3)]
        [InlineData(ImplKind.CSPoco, "abcdefg", 3)]
        [InlineData(ImplKind.JsonNewtonSoft, "abcdefg", 3)]
        [InlineData(ImplKind.MessagePack, "abcdefg", 3)]
        [InlineData(ImplKind.MemBlocks, "abcdefg", 3)]
        public void AddValue(ImplKind impl, string order, int expectedDepth)
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            Func<IBinaryTree<string, long>> nodeFactory = GetNodeFactory(impl);

            var tree = nodeFactory();
            tree.Count.ShouldBe(0);
            tree.Depth.ShouldBe(0);
            tree.DebugString().ShouldBe("");

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

            var pairs = tree.GetKeyValuePairs().ToArray();
            for (int i = 0; i < pairs.Length; i++)
            {
                if (i > 0)
                {
                    pairs[i].Key.ShouldBeGreaterThan(pairs[i - 1].Key);
                }
            }
            tree.Count.ShouldBe(count);
            tree.Depth.ShouldBe(expectedDepth);

            var node = tree.Get("b");
            node.ShouldNotBeNull();
            node.Key.ShouldBe("b");
            node.Value.ShouldBe(2L);

            node = tree.Get("z");
            node.ShouldBeNull();
        }

    }
}
