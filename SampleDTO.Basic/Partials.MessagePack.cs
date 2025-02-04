using MessagePack;
using System.Text;

namespace SampleDTO.Basic.MessagePack
{
    public partial class MyDTO
    {
        [IgnoreMember]
        public string? Field05
        {
            get
            {
                short length = this.Field05_Length;
                return length switch
                {
                    < 0 => null,
                    0 => string.Empty,
                    _ => Encoding.UTF8.GetString(Field05_Data.Span.Slice(0, length))
                };
            }
            set
            {
                if (value is null)
                {
                    this.Field05_Length = -1;
                }
                else if (value.Length == 0)
                {
                    this.Field05_Length = 0;
                }
                else
                {
                    var buffer = Encoding.UTF8.GetBytes(value);
                    this.Field05_Data = buffer;
                    this.Field05_Length = (short)buffer.Length;
                }
            }
        }
    }
}
