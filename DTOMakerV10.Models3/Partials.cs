using System.Text;

namespace DTOMakerV10.Models3.JsonSystemText
{
    public partial class Tree
    {
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("(Size=");
            result.Append(_Size);
            if (_Node is not null)
            {
                result.Append(",Node=");
                result.Append(_Node);
            }
            if (_Left is not null)
            {
                result.Append(",Left=");
                result.Append(_Left);
            }
            if (_Right is not null)
            {
                result.Append(",Right=");
                result.Append(_Right);
            }
            result.Append(')');
            return result.ToString();
        }
    }
    public partial class DoubleNode { public override string ToString() => $"(Value={_Value})"; }
    public partial class Int64Node { public override string ToString() => $"(Value={_Value})"; }
    public partial class StringNode { public override string ToString() => $"(Value={_Value})"; }
    public partial class BooleanNode { public override string ToString() => $"(Value={_Value})"; }
}