namespace SlimDownYourAggregates.Tests.Slimmed.Entities;

public class Format
{
    public Format(string formatType, int totalCopies, int soldCopies)
    {
        FormatType = formatType;
        TotalCopies = totalCopies;
        SoldCopies = soldCopies;
    }

    public string FormatType { get; }
    public int TotalCopies { get; }
    public int SoldCopies { get; }
}
