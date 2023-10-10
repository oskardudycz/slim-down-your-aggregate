using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.Factories;

public interface IBookFactory
{
    Book Create(
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
