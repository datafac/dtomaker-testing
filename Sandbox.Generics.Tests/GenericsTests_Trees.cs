using DataFac.Storage;
using DTOMaker.Runtime;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sandbox.Generics.Tests
{
    //internal sealed class nodeFactory_CSPoco : IBinaryTreeFactory<string, long>
    //{
    //    public IBinaryTree<string, long> CreateEmpty()
    //    {
    //        var result = new Sandbox.Generics.Models.CSPoco.MyTree();
    //        return result;
    //    }

    //    public IBinaryTree<string, long> CreateNode(string key, long value)
    //    {
    //        var result = new Sandbox.Generics.Models.CSPoco.MyTree();
    //        result.HasValue = true;
    //        result.Key = key;
    //        result.Value = value;
    //        result.Count = 1;
    //        result.Depth = 1;
    //        return result;
    //    }
    //}

    //internal sealed class nodeFactory_JsonSystemText : IBinaryTreeFactory<string, long>
    //{
    //    public IBinaryTree<string, long> CreateEmpty()
    //    {
    //        var result = new Sandbox.Generics.Models.JsonSystemText.MyTree();
    //        return result;
    //    }

    //    public IBinaryTree<string, long> CreateNode(string key, long value)
    //    {
    //        var result = new Sandbox.Generics.Models.JsonSystemText.MyTree();
    //        result.HasValue = true;
    //        result.Key = key;
    //        result.Value = value;
    //        result.Count = 1;
    //        result.Depth = 1;
    //        return result;
    //    }
    //}

    //internal sealed class nodeFactory_MemBlocks : IBinaryTreeFactory<string, long>
    //{
    //    public IBinaryTree<string, long> CreateEmpty()
    //    {
    //        var result = new Sandbox.Generics.Models.MemBlocks.MyTree();
    //        return result;
    //    }

    //    public IBinaryTree<string, long> CreateNode(string key, long value)
    //    {
    //        var result = new Sandbox.Generics.Models.MemBlocks.MyTree();
    //        result.HasValue = true;
    //        result.Key = key;
    //        result.Value = value;
    //        result.Count = 1;
    //        result.Depth = 1;
    //        return result;
    //    }
    //}

    //internal sealed class nodeFactory_MessagePack : IBinaryTreeFactory<string, long>
    //{
    //    public IBinaryTree<string, long> CreateEmpty()
    //    {
    //        var result = new Sandbox.Generics.Models.MessagePack.MyTree();
    //        return result;
    //    }

    //    public IBinaryTree<string, long> CreateNode(string key, long value)
    //    {
    //        var result = new Sandbox.Generics.Models.MessagePack.MyTree();
    //        result.HasValue = true;
    //        result.Key = key;
    //        result.Value = value;
    //        result.Count = 1;
    //        result.Depth = 1;
    //        return result;
    //    }
    //}

    //public class GenericsTests_Trees
    //{
    //    private static IBinaryTreeFactory<string, long> GetNodeFactory(ImplKind kind)
    //    {
    //        return kind switch
    //        {
    //            ImplKind.CSPoco => new nodeFactory_CSPoco(),
    //            ImplKind.JsonSystemText => new nodeFactory_JsonSystemText(),
    //            ImplKind.MemBlocks => new nodeFactory_MemBlocks(),
    //            ImplKind.MessagePack => new nodeFactory_MessagePack(),
    //            _ => throw new NotSupportedException($"Unsupported implementation kind: {kind}"),
    //        };
    //    }

    //    [Theory]
    //    [InlineData(ImplKind.CSPoco, "b", 1)]
    //    [InlineData(ImplKind.CSPoco, "ba", 2)]
    //    [InlineData(ImplKind.CSPoco, "bc", 2)]
    //    [InlineData(ImplKind.CSPoco, "abc", 2)]
    //    [InlineData(ImplKind.CSPoco, "acb", 3)]
    //    [InlineData(ImplKind.CSPoco, "bac", 2)]
    //    [InlineData(ImplKind.CSPoco, "bca", 2)]
    //    [InlineData(ImplKind.CSPoco, "cba", 2)]
    //    [InlineData(ImplKind.CSPoco, "cab", 3)]
    //    [InlineData(ImplKind.CSPoco, "dbacfeg", 3)]
    //    [InlineData(ImplKind.CSPoco, "abcdefg", 3)]
    //    [InlineData(ImplKind.JsonSystemText, "abcdefg", 3)]
    //    [InlineData(ImplKind.MessagePack, "abcdefg", 3)]
    //    [InlineData(ImplKind.MemBlocks, "abcdefg", 3)]
    //    public void AddValue(ImplKind impl, string order, short maxDepth)
    //    {
    //        using var dataStore = new DataFac.Storage.Testing.TestDataStore();

    //        var nodeFactory = GetNodeFactory(impl);

    //        var tree = nodeFactory.CreateEmpty();
    //        {
    //            if (tree is IPackable packable) packable.Pack(dataStore);
    //            tree.Freeze();
    //        }
    //        tree.Count.ShouldBe(0);
    //        tree.Depth.ShouldBe((short)0);

    //        // add nodes in order
    //        int count = 0;
    //        foreach (char ch in order)
    //        {
    //            long value = (Char.IsLetter(ch) && Char.IsLower(ch)) ? (ch - 'a') + 1 : throw new ArgumentException($"Unexpected character: {ch}");
    //            tree = tree.AddOrUpdate(new string(ch, 1), value, nodeFactory);
    //            count++;

    //            // pack and freeze the tree after each addition
    //            if (tree is IPackable packable) packable.Pack(dataStore);
    //            tree.Freeze();
    //        }


    //        var pairs = tree.GetKeyValuePairs().ToArray();
    //        for (int i = 0; i < pairs.Length; i++)
    //        {
    //            if (i > 0)
    //            {
    //                pairs[i].Key.ShouldBeGreaterThan(pairs[i - 1].Key);
    //            }
    //        }
    //        tree.Count.ShouldBe(count);
    //        tree.Depth.ShouldBeLessThanOrEqualTo(maxDepth);

    //        var node = tree.Get("b");
    //        node.ShouldNotBeNull();
    //        node.Key.ShouldBe("b");
    //        node.Value.ShouldBe(2L);

    //        node = tree.Get("z");
    //        node.ShouldBeNull();
    //    }

    //    private IEnumerable<string> GetCharCombinations(string chars)
    //    {
    //        if (chars.Length == 1)
    //        {
    //            yield return new string(chars[0], 1);
    //        }
    //        else
    //        {
    //            for (int i = 0; i < chars.Length; i++)
    //            {
    //                char ch = chars[i];
    //                string remaining = chars.ToString().Remove(i, 1);
    //                foreach (var subCombination in GetCharCombinations(remaining))
    //                {
    //                    yield return ch + subCombination;
    //                }
    //            }
    //        }
    //    }

    //    [Theory]
    //    [InlineData("ab", "ab,ba")]
    //    [InlineData("abc", "abc,acb,bac,bca,cab,cba")]
    //    [InlineData("abcd", "abcd,abdc,acbd,acdb,adbc,adcb,bacd,badc,bcad,bcda,bdac,bdca,cabd,cadb,cbad,cbda,cdab,cdba,dabc,dacb,dbac,dbca,dcab,dcba")]
    //    public void CheckCharCombinations(string input, string expected)
    //    {
    //        string[] combinations = GetCharCombinations(input).ToArray();
    //        string.Join(",", combinations).ShouldBe(expected);
    //    }

    //    [Theory]
    //    [InlineData("a", 1)]
    //    [InlineData("ab", 2)]
    //    [InlineData("abc", 3)]
    //    [InlineData("abcd", 3)]
    //    [InlineData("abcde", 4)]
    //    [InlineData("abcdef", 4)]
    //    [InlineData("abcdefg", 4)]
    //    public void AllCombinations(string chars, short maxDepth)
    //    {
    //        var nodeFactory = GetNodeFactory(ImplKind.CSPoco);
    //        using var dataStore = new DataFac.Storage.Testing.TestDataStore();

    //        foreach (string order in GetCharCombinations(chars))
    //        {
    //            var tree = nodeFactory.CreateEmpty();
    //            {
    //                if (tree is IPackable packable) packable.Pack(dataStore);
    //                tree.Freeze();
    //            }
    //            tree.Count.ShouldBe(0);
    //            tree.Depth.ShouldBe((short)0);

    //            // add nodes in order
    //            int count = 0;
    //            foreach (char ch in order)
    //            {
    //                long value = (Char.IsLetter(ch) && Char.IsLower(ch)) ? (ch - 'a') + 1 : throw new ArgumentException($"Unexpected character: {ch}");
    //                tree = tree.AddOrUpdate(new string(ch, 1), value, nodeFactory);
    //                count++;

    //                // pack and freeze the tree after each addition
    //                if (tree is IPackable packable) packable.Pack(dataStore);
    //                tree.Freeze();
    //            }


    //            var pairs = tree.GetKeyValuePairs().ToArray();
    //            for (int i = 0; i < pairs.Length; i++)
    //            {
    //                if (i > 0)
    //                {
    //                    pairs[i].Key.ShouldBeGreaterThan(pairs[i - 1].Key);
    //                }
    //            }
    //            tree.Count.ShouldBe(count);
    //            int depth = tree.Depth;
    //            depth.ShouldBeLessThanOrEqualTo(maxDepth, $"Input='{order}',depth={depth}");
    //        }
    //    }
    //}
}