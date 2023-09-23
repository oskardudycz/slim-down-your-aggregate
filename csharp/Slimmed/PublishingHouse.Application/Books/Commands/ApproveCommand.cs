using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record ApproveCommand(BookId BookId, CommitteeApproval CommitteeApproval);
