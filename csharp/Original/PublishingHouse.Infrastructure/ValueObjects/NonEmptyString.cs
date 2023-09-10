using PublishingHouse.Core.Validation;

namespace PublishingHouse.Core.ValueObjects;

public record NonEmptyString
{
    public NonEmptyString(string value) =>
        Value = value.AssertNotEmpty();

    public string Value { get; }
}
