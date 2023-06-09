namespace SlimDownYourAggregates.Tests.Original.Entities;

public class BookId
{
    public BookId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException();
        Value = value;
    }

    public Guid Value { get; }
}
