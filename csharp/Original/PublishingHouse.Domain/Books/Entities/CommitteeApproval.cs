using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public record CommitteeApproval(bool IsApproved, NonEmptyString Feedback);

