using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;

namespace SampleDTO.Strings
{
    [Entity]
    [EntityKey(2)]
    [Layout(LayoutMethod.Linear)]
    [Id("1f4ec791-e425-4388-bc0f-3434c15c6043")]
    public interface IStringsDTO
    {
        [Member(1)][StrLen(256)] string Field05_Value { get; set; }
        [Member(2)] bool Field05_HasValue { get; set; }
        string? Field05 { get; set; }
    }

}
