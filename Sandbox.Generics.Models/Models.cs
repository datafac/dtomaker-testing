using DTOMaker.Models;

namespace Sandbox.Generics.Models
{
    [Entity(1, LayoutMethod.Linear)]
    public interface IMyBinaryTree : IEntityBase
    {
        [Member(1)] int Count { get; set; }
        [Member(2)] byte Depth { get; set; }
        [Member(3)] string Key { get; set; }
        [Member(4)] long Value { get; set; }
        [Member(5)] IMyBinaryTree? Left { get; set; }
        [Member(6)] IMyBinaryTree? Right { get; set; }
    }

    //[Entity]
    //[Id(2)]
    //[Layout(LayoutMethod.Linear)]
    //public interface IMyStringList : ISkipList<string>
    //{
    //}
}
