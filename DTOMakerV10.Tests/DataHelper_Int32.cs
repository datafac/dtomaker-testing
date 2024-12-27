using DTOMakerV10.Models.MessagePack;
using System;

namespace DTOMakerV10.Tests
{
    internal class DataHelper_Int32 : IDataHelper<Int32, Data_Int32>
    {
        public static readonly DataHelper_Int32 Instance = new DataHelper_Int32();

        public Int32 CreateValue(ValueKind kind)
        {
            return kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.NegUnit => -1,
                ValueKind.MaxValue => Int32.MaxValue,
                ValueKind.MinValue => Int32.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
        }

        public Int32 GetValue(Data_Int32 message) => message.Value;
        public Data_Int32 NewClass(Int32 value) => new Data_Int32 { Value = value };
    }
    internal class DataHelper_Int16 : IDataHelper<Int16, Data_Int16>
    {
        public static readonly DataHelper_Int16 Instance = new DataHelper_Int16();

        public Int16 CreateValue(ValueKind kind)
        {
            return kind switch
            {
                ValueKind.Default => 0,
                ValueKind.PosUnit => 1,
                ValueKind.NegUnit => -1,
                ValueKind.MaxValue => Int16.MaxValue,
                ValueKind.MinValue => Int16.MinValue,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
        }

        public Int16 GetValue(Data_Int16 message) => message.Value;
        public Data_Int16 NewClass(Int16 value) => new Data_Int16 { Value = value };
    }
}