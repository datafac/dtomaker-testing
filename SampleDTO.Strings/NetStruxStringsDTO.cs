using DataFac.Memory;
using System;

namespace SampleDTO.Strings.NetStrux
{
    public sealed class NetStruxStringsDTO : IStringsDTO
    {
        private BlockB512 _block;

        public NetStruxStringsDTO() { }
        public bool TryRead(ReadOnlySpan<byte> source) => _block.TryRead(source);
        public bool TryWrite(Span<byte> target) => _block.TryWrite(target);
        public void Freeze() { }

        public string Field05_Value
        {
            get => _block.A.UTF8String;
            set => _block.A.UTF8String = value;
        }

        public bool Field05_HasValue
        {
            get => _block.B.A.A.A.A.A.A.A.A.BoolValue;
            set => _block.B.A.A.A.A.A.A.A.A.BoolValue = value;
        }

        public string? Field05
        {
            get
            {
                return Field05_HasValue ? Field05_Value : null;
            }
            set
            {
                if (value is null)
                {
                    Field05_HasValue = false;
                    Field05_Value = "";
                }
                else
                {
                    Field05_HasValue = true;
                    Field05_Value = value;
                }
            }
        }
    }

}
