using PublishingHouse.Core.Validation;

namespace PublishingHouse.Core.ValueObjects;

public record Ratio: IComparable<Ratio>
{
    public Ratio(decimal value)
    {
        Value = value.AssertBetween(0, 1);
    }

    public decimal Value { get; }

    public int CompareTo(Ratio? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }
}
