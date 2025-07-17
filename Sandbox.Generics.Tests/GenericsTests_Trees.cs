using DataFac.Storage;
using DTOMaker.Runtime;
using Newtonsoft.Json.Linq;
using Sandbox.Generics.Models;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Sandbox.Generics.Tests
{
    internal sealed class nodeFactory_CSPoco : IBinaryTreeFactory<string, long>
    {
        public IBinaryTree<string, long> CreateEmpty()
        {
            var result = new Sandbox.Generics.Models.CSPoco.MyTree();
            return result;
        }

        public IBinaryTree<string, long> CreateNode(string key, long value)
        {
            var result = new Sandbox.Generics.Models.CSPoco.MyTree();
            result.HasValue = true;
            result.Key = key;
            result.Value = value;
            result.Count = 1;
            result.Depth = 1;
            return result;
        }
    }

    internal sealed class nodeFactory_JsonNewtonSoft : IBinaryTreeFactory<string, long>
    {
        public IBinaryTree<string, long> CreateEmpty()
        {
            var result = new Sandbox.Generics.Models.JsonNewtonSoft.MyTree();
            return result;
        }

        public IBinaryTree<string, long> CreateNode(string key, long value)
        {
            var result = new Sandbox.Generics.Models.JsonNewtonSoft.MyTree();
            result.HasValue = true;
            result.Key = key;
            result.Value = value;
            result.Count = 1;
            result.Depth = 1;
            return result;
        }
    }

    internal sealed class nodeFactory_MemBlocks : IBinaryTreeFactory<string, long>
    {
        public IBinaryTree<string, long> CreateEmpty()
        {
            var result = new Sandbox.Generics.Models.MemBlocks.MyTree();
            return result;
        }

        public IBinaryTree<string, long> CreateNode(string key, long value)
        {
            var result = new Sandbox.Generics.Models.MemBlocks.MyTree();
            result.HasValue = true;
            result.Key = key;
            result.Value = value;
            result.Count = 1;
            result.Depth = 1;
            return result;
        }
    }

    internal sealed class nodeFactory_MessagePack : IBinaryTreeFactory<string, long>
    {
        public IBinaryTree<string, long> CreateEmpty()
        {
            var result = new Sandbox.Generics.Models.MessagePack.MyTree();
            return result;
        }

        public IBinaryTree<string, long> CreateNode(string key, long value)
        {
            var result = new Sandbox.Generics.Models.MessagePack.MyTree();
            result.HasValue = true;
            result.Key = key;
            result.Value = value;
            result.Count = 1;
            result.Depth = 1;
            return result;
        }
    }

    public class GenericsTests_Trees
    {
        private static IBinaryTreeFactory<string, long> GetNodeFactory(ImplKind kind)
        {
            return kind switch
            {
                ImplKind.CSPoco => new nodeFactory_CSPoco(),
                ImplKind.JsonNewtonSoft => new nodeFactory_JsonNewtonSoft(),
                ImplKind.MemBlocks => new nodeFactory_MemBlocks(),
                ImplKind.MessagePack => new nodeFactory_MessagePack(),
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

            var nodeFactory = GetNodeFactory(impl);

            var tree = nodeFactory.CreateEmpty();
            {
                if (tree is IPackable packable) packable.Pack(dataStore);
                if (tree is IFreezable freezable) freezable.Freeze();
            }
            tree.Count.ShouldBe(0);
            tree.Depth.ShouldBe(0);

            // add nodes in order
            int count = 0;
            foreach (char ch in order)
            {
                long value = (Char.IsLetter(ch) && Char.IsLower(ch)) ? (ch - 'a') + 1 : throw new ArgumentException($"Unexpected character: {ch}");
                tree = tree.AddOrUpdate(new string(ch, 1), value, nodeFactory);
                count++;

                // pack and freeze the tree after each addition
                if (tree is IPackable packable) packable.Pack(dataStore);
                if (tree is IFreezable freezable) freezable.Freeze();
            }


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
