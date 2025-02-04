using System.Text;

namespace SampleDTO.Basic.MemBlocks
{
    public partial class MyDTO
    {
        public string? Field05
        {
            get
            {
                short length = this.Field05_Length;
                if (length < 0) return null;
                else if (length == 0) return string.Empty;
                else
                {
                    return Encoding.UTF8.GetString(this.Field05_Data.Span.Slice(0, length));
                }
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
