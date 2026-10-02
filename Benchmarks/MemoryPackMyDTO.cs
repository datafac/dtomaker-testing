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
        public IEntityBase ShallowCopy()
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
        public IEntityBase ShallowCopy()
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
    public sealed partial class MemoryPackStringDTO : IStringDTO, IEquatable<MemoryPackStringDTO>
    {
        [MemoryPackInclude] public String? Field1 { get; set; }

        public void Freeze() { }
        public bool IsFrozen => false;
        public IEntityBase ShallowCopy() => new MemoryPackStringDTO { Field1 = Field1, };
        public bool Equals(MemoryPackStringDTO? other) { return other is not null && Field1 == other.Field1; }
        public override bool Equals(object? obj) => obj is MemoryPackStringDTO other && Equals(other);
        public override int GetHashCode() { return HashCode.Combine(typeof(MemoryPackStringDTO), Field1); }
    }

    [MemoryPackable]
    public sealed partial class TextTree : ITextTree, IEquatable<TextTree>, IBinaryTree<int, string, TextTree>
    {
        public void Freeze() { }
        public bool IsFrozen => false;
        public IEntityBase ShallowCopy()
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
        //[MemoryPackIgnore] IBinaryTree<int, string, TextTree>? IBinaryTree<int, string, TextTree>.Left { get => Left; set => Left = value is null ? null : (TextTree)value; }
        [MemoryPackInclude] public TextTree? Right { get; set; }
        [MemoryPackIgnore] ITextTree? ITextTree.Right { get => Right; set => Right = value is null ? null : (TextTree)value; }
        //[MemoryPackIgnore] IBinaryTree<int, string, TextTree>? IBinaryTree<int, string, TextTree>.Right { get => Right; set => Right = value is null ? null : (TextTree)value; }

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
