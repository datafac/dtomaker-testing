namespace SampleDTO.Strings.MemBlocks
{
    public partial class StringsDTO
    {
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
