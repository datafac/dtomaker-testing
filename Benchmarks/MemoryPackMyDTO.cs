using DataFac.Memory;
using DTOMaker.Runtime;
using MemoryPack;
using System;

namespace TestModels.MemPack
{
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(Quadrilateral))]
    [MemoryPackUnion(1, typeof(Rectangle))]
    public abstract partial class Shape : IShape
    {
        public void Freeze() { }
        public bool IsFrozen => false;
        public IEntityBase PartCopy()
        {
            throw new NotImplementedException();
        }
    }

    [MemoryPackable]
    [MemoryPackUnion(1, typeof(Rectangle))]
    public abstract partial class Quadrilateral : Shape, IQuadrilateral
    {
    }

    [MemoryPackable]
    public sealed partial class Rectangle : Quadrilateral, IRectangle
    {
        [MemoryPackInclude] public double Length { get; set; }
        [MemoryPackInclude] public double Height { get; set; }
    }

    [MemoryPackable]
    public sealed partial class MemoryPackMyDTO : IMyDTO
    {
        public void Freeze() { }
        public bool IsFrozen => false;
        public IEntityBase PartCopy()
        {
            return new MemoryPackMyDTO
            {
                Field01 = this.Field01,
                Field02LE = this.Field02LE,
                Field03BE = this.Field03BE,
                Field04 = this.Field04,
                Field05 = this.Field05,
                Field06 = this.Field06,
                Field07 = this.Field07,
                Field08 = this.Field08,
                Field09 = this.Field09
            };
        }

        [MemoryPackInclude] public bool Field01 { get; set; }
        [MemoryPackInclude] public double Field02LE { get; set; }
        [MemoryPackInclude] public double Field03BE { get; set; }
        [MemoryPackInclude] public Guid Field04 { get; set; }
        [MemoryPackInclude] public string? Field05 { get; set; }
        [MemoryPackInclude] public ReadOnlyMemory<byte> Field06 { get; set; }
        [MemoryPackInclude] public PairOfInt16 Field07 { get; set; }
        [MemoryPackInclude] public PairOfInt32 Field08 { get; set; }
        [MemoryPackInclude] public PairOfInt64 Field09 { get; set; }
        [MemoryPackIgnore] Octets? IMyDTO.Field06 { get => Octets.UnsafeWrap(this.Field06); set => this.Field06 = (value is null) ? default : value.AsMemory(); }
    }
}
