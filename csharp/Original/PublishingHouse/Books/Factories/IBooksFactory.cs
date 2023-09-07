using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Services;

namespace PublishingHouse.Books.Factories;

public interface IBooksFactory
{
    Book Create(
        BookId bookId,
        Book.State state,
        Title title,
        Author author,
        Genre? genre,
        IPublishingHouse publishingHouse,
        Publisher publisher,
        int edition,
        ISBN? isbn,
        DateTime? publicationDate,
        int? totalPages,
        int? numberOfIllustrations,
        string? bindingType,
        string? summary,
        CommitteeApproval? committeeApproval,
        List<Reviewer> reviewers,
        List<Chapter> chapters,
        List<Translation> translations,
        List<Format> formats
    );
}
