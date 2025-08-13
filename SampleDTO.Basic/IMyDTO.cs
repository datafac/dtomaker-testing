using DataFac.Memory;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Runtime;
using System;
using System.Numerics;

namespace SampleDTO.Basic
{
    [Entity]
    [Id(1)]
    [Layout(LayoutMethod.Linear)]
    public interface IMyDTO : IEntityBase
    {
        [Member(1)] bool Field01 { get; set; }
        [Member(2)][Endian(false)] double Field02LE { get; set; }
        [Member(3)][Endian(true)] double Field02BE { get; set; }
        [Member(4)] Guid Field03 { get; set; }
        [Member(5)] short Field05_Length { get; set; }
        [Member(6)][Capacity(128)] ReadOnlyMemory<byte> Field05_Data { get; set; }
        string? Field05 { get; set; }
        [Member(7)] PairOfInt16 Field07 { get; set; }
        //todo [Member(8)] DayOfWeek Field07 { get; set; }
    }

    public sealed class DayOfWeekConverter : ITypeConverter<DayOfWeek, int>
    {
        public DayOfWeek ToCustom(int native) => (DayOfWeek)native;
        public int ToNative(DayOfWeek custom) => (int)custom;
    }

    public sealed class VersionConverter : ITypeConverter<Version, string>
    {
        public Version ToCustom(string native) => Version.TryParse(native, out var version) ? version : new Version(0, 0);
        public string ToNative(Version custom) => custom.ToString();
    }

    public sealed class TimeSpanConverter : ITypeConverter<TimeSpan, long>
    {
        public TimeSpan ToCustom(long native) => TimeSpan.FromTicks(native);
        public long ToNative(TimeSpan custom) => custom.Ticks;
    }

    public sealed class DateTimeConverter : ITypeConverter<DateTime, long>
    {
        public DateTime ToCustom(long native) => DateTime.FromBinary(native);
        public long ToNative(DateTime custom) => custom.ToBinary();
    }

    public sealed class DateTimeOffsetConverter : ITypeConverter<DateTimeOffset, PairOfInt64>
    {
        public DateTimeOffset ToCustom(PairOfInt64 native) => new DateTimeOffset(DateTime.FromBinary(native.A), TimeSpan.FromTicks(native.B));
        public PairOfInt64 ToNative(DateTimeOffset custom) => new PairOfInt64(custom.DateTime.ToBinary(), custom.TimeOfDay.Ticks);
    }

#if NET8_0_OR_GREATER
    public sealed class ComplexConverter : ITypeConverter<Complex, Int128>
    {
        public Int128 ToNative(Complex custom)
        {
            BlockB016 orig = default;
            orig.A.UInt64ValueLE = BitConverter.DoubleToUInt64Bits(custom.Real);
            orig.B.UInt64ValueLE = BitConverter.DoubleToUInt64Bits(custom.Imaginary);
            return orig.Int128ValueLE;
        }

        public Complex ToCustom(Int128 native)
        {
            BlockB016 copy = default;
            copy.Int128ValueLE = native;
            return new Complex(
                BitConverter.UInt64BitsToDouble(copy.A.UInt64ValueLE),
                BitConverter.UInt64BitsToDouble(copy.B.UInt64ValueLE)
            );
        }
    }
#endif
}
