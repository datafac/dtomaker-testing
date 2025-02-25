using DataFac.Memory;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;

namespace SampleDTO.Binary
{
    [Entity]
    [EntityKey(2)]
    [Layout(LayoutMethod.Linear)]
    [Id("211b750e-5655-4e31-8b9e-22aba74311fe")]
    public interface IBinaryDTO
    {
        [Member(1)] Octets? Value { get; set; }
    }

}
