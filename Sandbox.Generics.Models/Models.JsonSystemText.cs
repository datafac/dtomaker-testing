using DTOMaker.Models.BinaryTree;

namespace Sandbox.Generics.Models.JsonSystemText
{
    public partial class MyBinaryTree : IBinaryTree<string, long>
    {
        IBinaryTree<string, long>? IBinaryTree<string, long>.Left
        {
            get => Left;
            set => Left = value is null ? null : new MyBinaryTree((IMyBinaryTree)value);
        }
        IBinaryTree<string, long>? IBinaryTree<string, long>.Right
        {
            get => Right;
            set => Right = value is null ? null : new MyBinaryTree((IMyBinaryTree)value);
        }
    }
}
