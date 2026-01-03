using DTOMaker.Models;
using DTOMaker.Runtime;

namespace Sandbox.Generics.Models
{
    [Entity(1, LayoutMethod.Linear)]
    public interface IMyBinaryTree : IEntityBase
    {
        [Member(1)] bool HasValue { get; set; }
        [Member(2)] byte Depthqqq { get; set; }
        [Member(3)] int Count { get; set; }
        [Member(4)] string Key { get; set; }
        [Member(5)] long Value { get; set; }
        [Member(6)] IMyBinaryTree? Left { get; set; }
        [Member(7)] IMyBinaryTree? Right { get; set; }
    }

    //[Entity]
    //[Id(2)]
    //[Layout(LayoutMethod.Linear)]
    //public interface IMyStringList : ISkipList<string>
    //{
    //}
}
