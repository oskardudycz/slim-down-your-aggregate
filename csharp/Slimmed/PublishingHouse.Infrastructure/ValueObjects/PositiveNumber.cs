using PublishingHouse.Core.Validation;

namespace PublishingHouse.Core.ValueObjects;

public record PositiveInt: IComparable<PositiveInt>
{
    public PositiveInt(int value) =>
        Value = value.AssertPositive();

    public int Value { get; }

    public int CompareTo(PositiveInt? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }
}
