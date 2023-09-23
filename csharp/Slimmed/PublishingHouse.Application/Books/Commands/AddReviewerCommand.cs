using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record AddReviewerCommand(BookId BookId, Reviewer Reviewer);
