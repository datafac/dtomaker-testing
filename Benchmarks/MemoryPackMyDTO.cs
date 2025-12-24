using DataFac.Memory;
using DTOMaker.Runtime;
using MemoryPack;
using System;

namespace TestModels.MemPack
{
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(Quadrilateral))]
    [MemoryPackUnion(1, typeof(Rectangle))]
    public abstract partial class Shape : IShape, IEquatable<Shape>
    {
        public void Freeze() { }
        public bool IsFrozen => false;
        public IEntityBase PartCopy()
        {
            throw new NotImplementedException();
        }
        #region IEquatable implementation
        public bool Equals(Shape? other) => other is not null;
        public override bool Equals(object? obj) => obj is Shape other && Equals(other);
        public override int GetHashCode()
        {
            HashCode hasher = new();
            hasher.Add(this.GetType());
            return hasher.ToHashCode();
        }
        #endregion
    }

    [MemoryPackable]
    [MemoryPackUnion(1, typeof(Rectangle))]
    public abstract partial class Quadrilateral : Shape, IQuadrilateral, IEquatable<Quadrilateral>
    {
        #region IEquatable implementation
        public bool Equals(Quadrilateral? other) => other is not null && base.Equals(other);
        public override bool Equals(object? obj) => obj is Quadrilateral other && Equals(other);
        #endregion
    }

    [MemoryPackable]
    public sealed partial class Rectangle : Quadrilateral, IRectangle, IEquatable<Rectangle>
    {
        [MemoryPackInclude] public double Length { get; set; }
        [MemoryPackInclude] public double Height { get; set; }

        #region IEquatable implementation
        public bool Equals(Rectangle? other)
        {
            return other is not null
                && base.Equals(other)
                && this.Length == other.Length
                && this.Height == other.Height;
        }

        public override bool Equals(object? obj) => obj is Rectangle other && Equals(other);
        public override int GetHashCode()
        {
            HashCode hasher = new();
            hasher.Add(base.GetHashCode());
            hasher.Add(Length);
            hasher.Add(Height);
            return hasher.ToHashCode();
        }
        #endregion
    }

    [MemoryPackable]
    public sealed partial class MemoryPackMyDTO : IMyDTO, IEquatable<MemoryPackMyDTO>
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

        #region IEquatable implementation
        public bool Equals(MemoryPackMyDTO? other)
        {
            return other is not null
                && this.Field01 == other.Field01
                && this.Field02LE == other.Field02LE
                && this.Field03BE == other.Field03BE
                && this.Field04 == other.Field04
                && this.Field05 == other.Field05
                && this.Field06.Span.SequenceEqual(other.Field06.Span)
                && this.Field07.Equals(other.Field07)
                && this.Field08.Equals(other.Field08)
                && this.Field09.Equals(other.Field09);
        }

        public override bool Equals(object? obj) => obj is MemoryPackMyDTO other && Equals(other);
        public override int GetHashCode()
        {
            HashCode hasher = new();
            hasher.Add(this.GetType());
            hasher.Add(Field01);
            hasher.Add(Field02LE);
            hasher.Add(Field03BE);
            hasher.Add(Field04);
            hasher.Add(Field05);
            hasher.Add(Field06.Length);
            hasher.AddBytes(Field06.Span);
            hasher.Add(Field07);
            hasher.Add(Field08);
            hasher.Add(Field09);
            return hasher.ToHashCode();
        }
        #endregion
    }
}
