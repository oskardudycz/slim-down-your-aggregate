using PublishingHouse.Application.Books.Commands;
using PublishingHouse.Persistence;
using PublishingHouse.Persistence.Books;
using PublishingHouse.Persistence.Books.Entities;

namespace PublishingHouse.Application.Books;

public class BooksService: IBooksService
{
    public void CreateDraft(CreateDraftCommand command)
    {
        PublishingHouseContext.Add(new BookEntity
        {
            Id = command.BookId,
            CurrentState = BookEntity.State.Writing,
            Title = "UNSET",
            Author = new Author("UNSET", "UNSET"),
            Publisher = "UNSET",
            Chapters = new List<Chapter>(),
            Formats = new List<Format>(),
            Reviewers = new List<Reviewer>(),
            Translations = new List<Translation>()
        });
    }
}
