using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using DataFac.Storage;
using DataFac.Storage.Testing;
using DTOMaker.Models.BinaryTree;
using DTOMaker.Runtime.MsgPack2;
using MemoryPack;
using System;
using System.Threading.Tasks;

namespace Benchmarks
{
    //[SimpleJob(RuntimeMoniker.Net80)]
    //[SimpleJob(RuntimeMoniker.Net90)]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DTORoundtripBinaryTree
    {
        /// <summary>
        /// Unit tests should set this to true to validate that the values are correctly roundtripped.
        /// </summary>
        public bool CheckValues = false;

        private readonly TestDataStore DataStore = new TestDataStore();

        public Counters GetCounters() => DataStore.GetCounters();
        public void ResetCounters() => DataStore.ResetCounters();

        private static T Populate<T>() where T : class, IBinaryTree<int, string>, new()
        {
            static IBinaryTree<int, string> newNodeFn() => new T();
            IBinaryTree<int, string>? tree = null;
            tree = tree.AddOrUpdate(1, "Alice", newNodeFn);
            tree = tree.AddOrUpdate(2, "Brett", newNodeFn);
            tree = tree.AddOrUpdate(3, "Chloe", newNodeFn);
            return tree as T ?? throw new InvalidOperationException("Tree is null after adding nodes");
        }

        [Benchmark(Baseline = true)]
        public int BinaryTree_MemoryPack()
        {
            var orig = Populate<TestModels.MemPack.TextTree>();
            orig.Freeze();
            ReadOnlyMemory<byte> buffer = MemoryPackSerializer.Serialize<TestModels.MemPack.TextTree>(orig);
            var copy = MemoryPackSerializer.Deserialize<TestModels.MemPack.TextTree>(buffer.Span);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public async ValueTask<int> BinaryTree_MemBlocks()
        {
            var orig = Populate<TestModels.MemBlocks.TextTree>();
            await orig.Pack(DataStore);
            var buffers = orig.GetBuffers();
            var copy = TestModels.MemBlocks.TextTree.DeserializeFrom(buffers);
            if (CheckValues)
            {
                //await copy.UnpackAll(DataStore);
                if (!copy.Equals(orig))
                {
                    throw new Exception("Roundtrip values do not match");
                }
            }
            return 0;
        }

        [Benchmark]
        public int BinaryTree_MsgPack2()
        {
            var orig = Populate<TestModels.MsgPack2.TextTree>();
            orig.Freeze();
            ReadOnlyMemory<byte> buffer = orig.SerializeToMessagePack<TestModels.MsgPack2.TextTree>();
            var copy = buffer.DeserializeFromMessagePack<TestModels.MsgPack2.TextTree>();
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public int BinaryTree_JsonSystemText()
        {
            var orig = Populate<TestModels.JsonSystemText.TextTree>();
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.SerializeToJson<TestModels.JsonSystemText.TextTree>(orig);
            var copy = DTOMaker.Runtime.JsonSystemText.SerializationHelpers.DeserializeFromJson<TestModels.JsonSystemText.TextTree>(buffer);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }

        [Benchmark]
        public int BinaryTree_JsonNewtonSoft()
        {
            var orig = Populate<TestModels.JsonNewtonSoft.TextTree>();
            orig.Freeze();
            string buffer = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.SerializeToJson<TestModels.JsonNewtonSoft.TextTree>(orig);
            var copy = DTOMaker.Runtime.JsonNewtonSoft.SerializationHelpers.DeserializeFromJson<TestModels.JsonNewtonSoft.TextTree>(buffer);
            copy!.Freeze();
            if (CheckValues && !copy.Equals(orig))
                throw new Exception("Roundtrip values do not match");
            return buffer.Length;
        }
    }
}
