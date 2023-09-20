using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Factories;

public interface IBookFactory
{
    Book Create(
        BookId bookId,
        Book.State state,
        Title title,
        Author author,
        Genre? genre,
        ISBN? isbn,
        CommitteeApproval? committeeApproval,
        List<Reviewer> reviewers,
        List<Chapter> chapters,
        List<Translation> translations,
        List<Format> formats
    );
}
