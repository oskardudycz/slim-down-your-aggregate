namespace PublishingHouse.Persistence.Books.ValueObjects;

public class CommitteeApprovalVO
{
    public CommitteeApprovalVO(bool isApproved, string feedback)
    {
        IsApproved = isApproved;
        Feedback = feedback;
    }

    public bool IsApproved { get; }
    public string Feedback { get; }
}

