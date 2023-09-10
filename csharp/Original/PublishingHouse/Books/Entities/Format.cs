using PublishingHouse.Core.Validation;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public record Format
{
    public Format(FormatType formatType, PositiveInt totalCopies, PositiveInt soldCopies)
    {
        FormatType = formatType;
        TotalCopies = totalCopies;
        SoldCopies = soldCopies;

        TotalCopies.AssertGreaterOrEqualThan(SoldCopies);
    }

    public FormatType FormatType { get; }
    public PositiveInt TotalCopies { get; }
    public PositiveInt SoldCopies { get; }
}

public record FormatType(string Value): NonEmptyString(Value);
