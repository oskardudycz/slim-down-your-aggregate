namespace PublishingHouse.Persistence.Books.ValueObjects;

public class CommitteeApproval
{
    public CommitteeApproval(bool isApproved, string feedback)
    {
        IsApproved = isApproved;
        Feedback = feedback;
    }

    public bool IsApproved { get; }
    public string Feedback { get; }
}

