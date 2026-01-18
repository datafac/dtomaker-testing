namespace Benchmarks
{
    public enum ValueKind
    {
        Bool,
        Int32LE,
        DoubleLE,
        Guid,
        PairOfInt16,
        PairOfInt32,
        PairOfInt64,
        StringNull,
        StringEmpty,
        StringSmall, // 32 chars
        StringLarge, // 1K chars
        BinaryNull,
        BinaryEmpty,
        BinarySmall,
        BinaryLarge,
        // custom
        //DayOfWeek,
        AllPropsSet,
    }
}
