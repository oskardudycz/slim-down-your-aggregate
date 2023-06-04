namespace SlimDownYourAggregates.Tests.Slimmed.Entities;

public class BookId
{
    public BookId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }
}
