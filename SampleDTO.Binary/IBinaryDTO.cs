using DataFac.Memory;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace SampleDTO.Binary
{
    [Entity]
    [Id(2)]
    [Layout(LayoutMethod.Linear)]
    public interface IBinaryDTO
    {
        [Member(1)] Octets? Value { get; }
    }

}
