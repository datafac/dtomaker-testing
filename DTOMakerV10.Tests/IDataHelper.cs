namespace DTOMakerV10.Tests
{
    internal interface IDataHelper<TValue, TClass>
    {
        TValue CreateValue(ValueKind kind);
        TClass NewClass(TValue value);
        TValue GetValue(TClass message);
    }
}