namespace PublishingHouse.Persistence.Books.Entities;

public class FormatEntity
{
    public required string FormatType { get; set; }
    public int TotalCopies { get; set; }
    public int SoldCopies { get; set; }
}
