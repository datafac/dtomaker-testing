using DataFac.Memory;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Runtime;

namespace SampleDTO.Binary
{
    [Entity]
    [Id(2)]
    [Layout(LayoutMethod.Linear)]
    public interface IBinaryDTO : IEntityBase
    {
        [Member(1)] Octets? Value { get; set; }
    }

}
