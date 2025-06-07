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
        [InlineData(ImplKind.MessagePack)]
        [InlineData(ImplKind.MemBlocks)]
        public void AddValue(ImplKind impl)
        {
            using var dataStore = new DataFac.Storage.Testing.TestDataStore();

            Func<ITree<string, long>> nodeFactory = impl switch
            {
                ImplKind.MessagePack => () => new Sandbox.Generics.Models.MessagePack.MyTree(),
                ImplKind.MemBlocks => () => new Sandbox.Generics.Models.MemBlocks.MyTree(),
                _ => throw new NotSupportedException($"Unsupported implementation kind: {impl}"),
            };

            var node = nodeFactory();
            node = node.AddValue("a", 1, nodeFactory);
            node = node.AddValue("b", 2, nodeFactory);
            node = node.AddValue("c", 3, nodeFactory);

            if (node is IPackable packable)
            {
                packable.Pack(dataStore);
            }

            if (node is IFreezable freezable)
            {
                freezable.Freeze();
            }

            node.Count.ShouldBe(3);
            node.Key.ShouldBe("a");
            node.Value.ShouldBe(1L);
            node.Left.ShouldBeNull();
            node.Right.ShouldNotBeNull();
            node.Right.Count.ShouldBe(2);
            node.Right.Key.ShouldBe("b");
            node.Right.Value.ShouldBe(2L);
            node.Right.Left.ShouldBeNull();
            node.Right.Right.ShouldNotBeNull();
            node.Right.Right.Count.ShouldBe(1);
            node.Right.Right.Key.ShouldBe("c");
            node.Right.Right.Value.ShouldBe(3L);
            node.Right.Right.Left.ShouldBeNull();
            node.Right.Right.Right.ShouldBeNull();
        }
    }
}
