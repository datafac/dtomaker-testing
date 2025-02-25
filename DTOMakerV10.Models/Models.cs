using System;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;

namespace DTOMakerV10.Models
{
    [Entity][EntityKey(1)][Layout(LayoutMethod.Linear)][Id("dfc8c28c-be1c-47b2-84a6-00e042796fc9")]
    public interface IData_SByte { [Member(1)] sbyte Value { get; set; } }

    [Entity][EntityKey(2)][Layout(LayoutMethod.Linear)][Id("1624239c-7d39-40b0-9a04-26a6d5061949")]
    public interface IData_Byte { [Member(1)] byte Value { get; set; } }

    [Entity][EntityKey(3)][Layout(LayoutMethod.Linear)][Id("dae1c012-e245-4192-a80e-09ac5ff4f745")]
    public interface IData_Int16 { [Member(1)] short Value { get; set; } }

    [Entity][EntityKey(4)][Layout(LayoutMethod.Linear)][Id("a95bd48f-821e-404b-af05-9127f101c6e4")]
    public interface IData_UInt16 { [Member(1)] ushort Value { get; set; } }

    [Entity][EntityKey(5)][Layout(LayoutMethod.Linear)][Id("c59f5288-4163-4e56-89db-8ae330cab66f")]
    public interface IData_Int32 { [Member(1)] int Value { get; set; } }

    [Entity][EntityKey(6)][Layout(LayoutMethod.Linear)][Id("3154090e-00fa-41d3-a89c-9e015ecac526")]
    public interface IData_UInt32 { [Member(1)] uint Value { get; set; } }

    [Entity][EntityKey(7)][Layout(LayoutMethod.Linear)][Id("1a689ee2-0a91-498c-84c4-feb8c81c4c10")]
    public interface IData_Int64 { [Member(1)] long Value { get; set; } }

    [Entity][EntityKey(8)][Layout(LayoutMethod.Linear)][Id("a2178cd5-1a38-4d82-a381-8737d58ff20e")]
    public interface IData_UInt64 { [Member(1)] ulong Value { get; set; } }

    [Entity][EntityKey(9)][Layout(LayoutMethod.Linear)][Id("463d4645-707c-4ed1-983c-34ea6709540d")]
    public interface IData_Boolean { [Member(1)] bool Value { get; set; } }

    [Entity][EntityKey(10)][Layout(LayoutMethod.Linear)][Id("34e55603-f8b1-44f1-8a02-7a7f21496007")]
    public interface IData_Char { [Member(1)] char Value { get; set; } }

    [Entity][EntityKey(11)][Layout(LayoutMethod.Linear)][Id("70e1b931-85c1-4252-8574-a3da444eaafe")]
    public interface IData_Single { [Member(1)] float Value { get; set; } }

    [Entity][EntityKey(12)][Layout(LayoutMethod.Linear)][Id("ec46c490-d97a-410e-b9d4-8c97898c06cf")]
    public interface IData_Double { [Member(1)] double Value { get; set; } }

    [Entity][EntityKey(13)][Layout(LayoutMethod.Linear)][Id("a48f192c-4201-4c90-8b89-005f9908d132")]
    public interface IData_Decimal { [Member(1)] decimal Value { get; set; } }

#if NET7_0_OR_GREATER
    [Entity][EntityKey(14)][Layout(LayoutMethod.Linear)][Id("6d46fd99-cd10-4152-967d-c7a4e39d38b6")]
    public interface IData_Int128 { [Member(1)] Int128 Value { get; set; } }

    [Entity][EntityKey(15)][Layout(LayoutMethod.Linear)][Id("1e05bb4c-fe85-4359-9673-c2e0ec4d6000")]
    public interface IData_UInt128 { [Member(1)] UInt128 Value { get; set; } }
#endif

    [Entity][EntityKey(16)][Layout(LayoutMethod.Linear)][Id("6576e9e2-4641-4027-9fcb-f9e5a752d26d")]
    public interface IData_Guid { [Member(1)] Guid Value { get; set; } }

#if NET5_0_OR_GREATER
    [Entity][EntityKey(21)][Layout(LayoutMethod.Linear)][Id("7bcae077-bbeb-4b6a-bb8e-646c73eb42f9")]
    public interface IData_Half { [Member(1)] Half Value { get; set; } }
#endif


    // todo Guid, Int128, UInt128
    // todo String
}