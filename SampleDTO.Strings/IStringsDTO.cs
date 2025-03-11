using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;

namespace SampleDTO.Strings
{
    [Entity]
    [Id(2)]
    [Layout(LayoutMethod.Linear)]
    public interface IStringsDTO
    {
        [Member(1)][FixedLength(256)] string Field05_Value { get; }
        [Member(2)] bool Field05_HasValue { get; }
        string? Field05 { get; set; }
    }

}
