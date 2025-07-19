using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace Sandbox.Generics.Models
{
    [Entity]
    [Id(1)]
    [Layout(LayoutMethod.Linear)]
    public interface IMyTree : IBinaryTree<string, long>
    {

    }

    [Entity]
    [Id(2)]
    [Layout(LayoutMethod.Linear)]
    public interface IMyStringList : ISkipList<string>
    {

    }
}
