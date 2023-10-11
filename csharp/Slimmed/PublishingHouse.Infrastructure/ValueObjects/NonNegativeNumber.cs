using PublishingHouse.Core.Validation;

namespace PublishingHouse.Core.ValueObjects;

public record NonNegativeNumber: IComparable<NonNegativeNumber>
{
    public NonNegativeNumber(int value) =>
        Value = value.AssertGreaterOrEqualThan(0);

    public int Value { get; }

    public int CompareTo(NonNegativeNumber? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }
}
