using MemoryPack;

namespace SampleDTO.Strings.MemoryPack
{
    [MemoryPackable]
    public sealed partial class MemoryPackStringsDTO : IStringsDTO
    {
        public void Freeze() { }

        [MemoryPackInclude] public string Field05_Value { get; set; } = "";
        [MemoryPackInclude] public bool Field05_HasValue { get; set; }

        [MemoryPackIgnore]
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
