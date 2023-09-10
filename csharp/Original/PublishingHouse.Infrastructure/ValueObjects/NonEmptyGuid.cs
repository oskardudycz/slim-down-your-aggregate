using PublishingHouse.Core.Validation;

namespace PublishingHouse.Core.ValueObjects;

public record NonEmptyGuid
{
    protected NonEmptyGuid(Guid value) =>
        Value = value.AssertNotEmpty();

    public Guid Value { get; }
}
