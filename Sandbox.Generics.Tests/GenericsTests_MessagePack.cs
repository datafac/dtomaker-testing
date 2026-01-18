namespace Sandbox.Generics.Tests
{
    //public class GenericsTests_MessagePack
    //{
    //    [Fact]
    //    public void Roundtrip1_Simple()
    //    {
    //        var sendMsg = new MyTree()
    //        {
    //            Count = 1,
    //            Depth = 1,
    //            Key = "abc",
    //            Value = 456L,
    //            Left = null,
    //            Right = null,
    //        };
    //        sendMsg.Freeze();

    //        // act
    //        ReadOnlyMemory<byte> buffer = sendMsg.SerializeToMessagePack<MyTree>();
    //        MyTree recdMsg = buffer.DeserializeFromMessagePack<MyTree>();
    //        recdMsg.Freeze();

    //        // assert
    //        // - equality
    //        recdMsg.ShouldNotBeNull();
    //        recdMsg!.Equals(sendMsg).ShouldBeTrue();
    //        recdMsg.ShouldBe(sendMsg);
    //        recdMsg.GetHashCode().ShouldBe(sendMsg.GetHashCode());
    //    }
    //}
}
