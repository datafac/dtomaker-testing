using DataFac.Memory;
using System;
using System.Text;

namespace SampleDTO.Basic.NetStrux
{
    public sealed class NetStruxMyDTO : IMyDTO
    {
        // -------------------- field map -----------------------------
        //  Seq.  Off.  Len.  N.    Type    End.  Name
        //  ----  ----  ----  ----  ------- ----  -------
        //     1     0     1        Boolean LE    Field01
        //     2     8     8        Double  LE    Field02LE
        //     3    16     8        Double  BE    Field02BE
        //     4    32    16        Guid    LE    Field03
        //     5    48     2        Int16   LE    Field05_Length
        //     6   128     1   128  Byte    LE    Field05_Data
        // ------------------------------------------------------------

        private BlockB256 _block;

        public NetStruxMyDTO() { }
        public bool TryRead(ReadOnlySpan<byte> source) => _block.TryRead(source);
        public bool TryWrite(Span<byte> target) => _block.TryWrite(target);
        public void Freeze() { }
        public bool Field01
        {
            get { return _block.A.A.A.A.A.A.A.A.BoolValue; }
            set { _block.A.A.A.A.A.A.A.A.BoolValue = value; }
        }

        public double Field02LE
        {
            get { return _block.A.A.A.B.A.DoubleValueLE; }
            set { _block.A.A.A.B.A.DoubleValueLE = value; }
        }

        public double Field02BE
        {
            get { return _block.A.A.A.B.B.DoubleValueBE; }
            set { _block.A.A.A.B.B.DoubleValueBE = value; }
        }

        public Guid Field03
        {
            get => _block.A.A.B.A.GuidValueLE;
            set => _block.A.A.B.A.GuidValueLE = value;
        }

        public short Field05_Length
        {
            get => _block.A.A.B.B.A.A.A.Int16ValueLE;
            set => _block.A.A.B.B.A.A.A.Int16ValueLE = value;
        }

        public ReadOnlyMemory<byte> Field05_Data
        {
            get
            {
                Span<byte> buffer = stackalloc byte[128];
                _block.B.TryWrite(buffer);
                return buffer.ToArray();
            }

            set
            {
                _block.B.TryRead(value.Span);
            }
        }
        private void Set_Field05_Data(ReadOnlySpan<byte> value)
        {
            _block.B.TryRead(value);
        }
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
                    Span<byte> buffer = stackalloc byte[128];
                    int length = Encoding.UTF8.GetBytes(value.AsSpan(), buffer);
                    this.Set_Field05_Data(buffer);
                    this.Field05_Length = (short)length;
                }
            }
        }
    }
}
