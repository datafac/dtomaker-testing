namespace Benchmarks
{
    public enum ValueKind
    {
        Bool,
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
        AllPropsSet,
    }
}
