using DTOMaker.Models;
using MasterMemory;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Xperimental.CySharp
{
    public enum Gender : int
    {
        Unknown, Male, Female
    }

    public sealed class GenderConverter : IStructConverter<Gender, int>
    {
        public NativeType NativeType => NativeType.Int32;
        public static Gender ToCustom(int native) => (Gender)native;
        public static int ToNative(Gender custom) => (int)custom;
        public static Gender? ToCustom(int? native) => native.HasValue ? ToCustom(native.Value) : null;
        public static int? ToNative(Gender? custom) => custom.HasValue ? ToNative(custom.Value) : null;
    }

    public interface INativeStruct<T> where T : struct, IEquatable<T>, IComparable<T>
    {
        T Native { get; }
    }
    public readonly struct PersonId : IEquatable<PersonId>, IComparable<PersonId>, INativeStruct<int>
    {
        private readonly int _native;
        public int Native => _native;
        public PersonId(int value) => _native = value;

        public int CompareTo(PersonId other) => _native.CompareTo(other._native);
        public bool Equals(PersonId other) => _native == other._native;
        public override bool Equals(object? obj) => obj is PersonId other && Equals(other);
        public override int GetHashCode() => _native.GetHashCode();
        public static bool operator ==(PersonId left, PersonId right) => left.Equals(right);
        public static bool operator !=(PersonId left, PersonId right) => !left.Equals(right);
    }

    public sealed class PersonIdConverter : IStructConverter<PersonId, int>
    {
        public NativeType NativeType => NativeType.Int32;
        public static PersonId ToCustom(int native) => new PersonId(native);
        public static int ToNative(PersonId custom) => custom.Native;
        public static PersonId? ToCustom(int? native) => native.HasValue ? ToCustom(native.Value) : null;
        public static int? ToNative(PersonId? custom) => custom.HasValue ? ToNative(custom.Value) : null;
    }

    [Entity(1)]
    public interface IPerson : IEntityBase
    {
        [Member(1, NativeType.Int32, typeof(PersonIdConverter))] PersonId Id { get; }
        [Member(2)] int Age { get; }
        [Member(3, NativeType.Int32, typeof(GenderConverter))] Gender Gender { get; }
        [Member(4)] string Name { get; }
    }
}

namespace Xperimental.CySharp.MemTables
{
    // table definition marked by MemoryTableAttribute.
    // database-table must be serializable by MessagePack-CSsharp
    [MemoryTable("person")]
    public sealed record MMPerson : IPerson
    {
        private volatile bool _isFrozen;
        public bool IsFrozen => _isFrozen;
        public void Freeze() => _isFrozen = true;
        public IEntityBase PartCopy() => new MMPerson() { Id = this.Id, Age = this.Age, Gender = this.Gender, Name = this.Name };

        public MMPerson() { }

        public MMPerson(IPerson source)
        {
            Id = source.Id;
            Age = source.Age;
            Gender = source.Gender;
            Name = source.Name;
        }

        // index definition by attributes.
        [PrimaryKey]
        public PersonId Id { get; init; }

        // secondary index can add multiple(discriminated by index-number).
        [SecondaryKey(0), NonUnique]
        [SecondaryKey(1, keyOrder: 1), NonUnique]
        public int Age { get; init; }

        [SecondaryKey(2), NonUnique]
        [SecondaryKey(1, keyOrder: 0), NonUnique]
        public Gender Gender { get; init; }

        public string Name { get; init; } = string.Empty;

    }
}
