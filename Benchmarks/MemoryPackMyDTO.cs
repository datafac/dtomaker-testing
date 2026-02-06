using DataFac.Memory;
using DTOMaker.Models;
using DTOMaker.Models.BinaryTree;
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
            hasher.Add(GetType());
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
        public override int GetHashCode() => base.GetHashCode();
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
                && Length == other.Length
                && Height == other.Height;
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
    public sealed partial class MemoryPackCustom1 : ICustom1, IEquatable<MemoryPackCustom1>
    {
        public void Freeze() { }
        public bool IsFrozen => false;
        public IEntityBase PartCopy()
        {
            return new MemoryPackCustom1
            {
                Field1 = Field1,
            };
        }

        [MemoryPackInclude] public DayOfWeek Field1 { get; set; }

        #region IEquatable implementation
        public bool Equals(MemoryPackCustom1? other)
        {
            return other is not null
                && Field1 == other.Field1
                ;
        }

        public override bool Equals(object? obj) => obj is MemoryPackCustom1 other && Equals(other);
        public override int GetHashCode()
        {
            HashCode hasher = new();
            hasher.Add(GetType());
            hasher.Add(Field1);
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
                FBool01 = FBool01,
                FSInt04 = FSInt04,
                Field02LE = Field02LE,
                Field03BE = Field03BE,
                Field04 = Field04,
                Field05 = Field05,
                Field06 = Field06,
                Field07 = Field07,
                Field08 = Field08,
                Field09 = Field09
            };
        }

        [MemoryPackInclude] public bool FBool01 { get; set; }
        [MemoryPackInclude] public int FSInt04 { get; set; }
        [MemoryPackInclude] public double Field02LE { get; set; }
        [MemoryPackInclude] public double Field03BE { get; set; }
        [MemoryPackInclude] public Guid Field04 { get; set; }
        [MemoryPackInclude] public string? Field05 { get; set; }
        [MemoryPackInclude] public ReadOnlyMemory<byte>? Field06 { get; set; }
        [MemoryPackInclude] public PairOfInt16 Field07 { get; set; }
        [MemoryPackInclude] public PairOfInt32 Field08 { get; set; }
        [MemoryPackInclude] public PairOfInt64 Field09 { get; set; }
        [MemoryPackIgnore]
        Octets? IMyDTO.Field06
        {
            get => Field06 is null ? null : Octets.UnsafeWrap(Field06.Value);
            set => Field06 = (value is null) ? null : value.AsMemory();
        }

        #region IEquatable implementation
        private static bool MemoryEqual(ReadOnlyMemory<byte>? a, ReadOnlyMemory<byte>? b)
        {
            return a is null ? b is null : b is not null && a.Value.Span.SequenceEqual(b.Value.Span);
        }

        public bool Equals(MemoryPackMyDTO? other)
        {
            return other is not null
                && FBool01 == other.FBool01
                && FSInt04 == other.FSInt04
                && Field02LE == other.Field02LE
                && Field03BE == other.Field03BE
                && Field04 == other.Field04
                && Field05 == other.Field05
                && MemoryEqual(Field06, other.Field06)
                && Field07.Equals(other.Field07)
                && Field08.Equals(other.Field08)
                && Field09.Equals(other.Field09);
        }

        public override bool Equals(object? obj) => obj is MemoryPackMyDTO other && Equals(other);
        public override int GetHashCode()
        {
            HashCode hasher = new();
            hasher.Add(typeof(MemoryPackMyDTO));
            hasher.Add(FBool01);
            hasher.Add(FSInt04);
            hasher.Add(Field02LE);
            hasher.Add(Field03BE);
            hasher.Add(Field04);
            hasher.Add(Field05);
            if (Field06 is not null)
            {
                hasher.Add(Field06.Value.Length);
                hasher.AddBytes(Field06.Value.Span);
            }
            hasher.Add(Field07);
            hasher.Add(Field08);
            hasher.Add(Field09);
            return hasher.ToHashCode();
        }
        #endregion
    }

    [MemoryPackable]
    public sealed partial class TextTree : ITextTree, IEquatable<TextTree>, IBinaryTree<int, string>
    {
        public void Freeze() { }
        public bool IsFrozen => false;
        public IEntityBase PartCopy()
        {
            return new TextTree
            {
                Count = this.Count,
                Depth = this.Depth,
                Key = this.Key,
                Value = this.Value,
                Left = this.Left,
                Right = this.Right,
            };
        }

        [MemoryPackInclude] public string Value { get; set; } = string.Empty;
        [MemoryPackInclude] public int Key { get; set; }
        [MemoryPackInclude] public int Count { get; set; }
        [MemoryPackInclude] public byte Depth { get; set; }
        [MemoryPackInclude] public TextTree? Left { get; set; }
        [MemoryPackIgnore] ITextTree? ITextTree.Left { get => Left; set => Left = value is null ? null : (TextTree)value; }
        [MemoryPackIgnore] IBinaryTree<int, string>? IBinaryTree<int, string>.Left { get => Left; set => Left = value is null ? null : (TextTree)value; }
        [MemoryPackInclude] public TextTree? Right { get; set; }
        [MemoryPackIgnore] ITextTree? ITextTree.Right { get => Right; set => Right = value is null ? null : (TextTree)value; }
        [MemoryPackIgnore] IBinaryTree<int, string>? IBinaryTree<int, string>.Right { get => Right; set => Right = value is null ? null : (TextTree)value; }

        public bool Equals(TextTree? other)
        {
            return other is not null
                && Count == other.Count
                && Depth == other.Depth
                && Key == other.Key
                && Value == other.Value
                && Left == other.Left
                && Right == other.Right
                ;
        }

        public override bool Equals(object? obj) => obj is TextTree other && Equals(other);
        public override int GetHashCode()
        {
            HashCode hasher = new();
            hasher.Add(typeof(TextTree));
            hasher.Add(Count);
            hasher.Add(Depth);
            hasher.Add(Key);
            hasher.Add(Value);
            return hasher.ToHashCode();
        }

        public static bool operator ==(TextTree? left, TextTree? right) => left is null ? right is null : left.Equals(right);
        public static bool operator !=(TextTree? left, TextTree? right) => left is null ? right is not null : !left.Equals(right);
    }
}
