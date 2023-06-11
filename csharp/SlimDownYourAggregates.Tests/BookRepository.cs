using System.Text.Json;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using SlimDownYourAggregates.Tests.Original;
using SlimDownYourAggregates.Tests.Original.Entities;

namespace SlimDownYourAggregates.Tests;

public interface IBookRepository
{
    ValueTask<BookModel> Find(BookId id);
    void Store(BookEvent @event);
    Task Save();
}

public interface IAmFancyOrm
{
    ValueTask<T> Find<T>(object id);
    void Store<T>(T model);
    Task Save();
}

public class ORMBookRepository: IBookRepository
{
    private readonly IAmFancyOrm orm;

    public ORMBookRepository(IAmFancyOrm orm)
    {
        this.orm = orm;
    }

    public async ValueTask<Book> Find(BookId id)
    {
        var model = await orm.Find<BookModel>(id.Value);

        var aggregate = (Book)(object)model;

        return aggregate!;
    }

    public void Store(BookModel model, BookEvent @event)
    {
        orm.Store(model);
        orm.Store(new Message(JsonSerializer.Serialize(@event));
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }
}

class ApplicationService
{
    private readonly IBookRepository repository;

    public ApplicationService(IBookRepository repository)
    {
        this.repository = repository;
    }

    public async ValueTask HandleAdd(BookId id)
    {
        var aggregate = new Book(
            new BookId(model.Id),
            model.Title,
            model.Author,
            model.Genre,
            model.Reviewers,
            model.ISBN
        );

        //aggregate.AddChapter();

        repository.Store(model);
        await repository.Save();
    }

    public async ValueTask AddChapter(BookId id)
    {
        var model = await repository.Find(id);

        var aggregate = new Book(
            new BookId(model.Id),
            model.Title,
            model.Author,
            model.Genre,
            model.Reviewers,
            model.ISBN
        );

        var @event = aggregate.AddChapter();

        repository.Store(Evolve(model, @event));
        await repository.Save();
    }

    public static BookModel Evolve(BookModel mongo, BookEvent @event)
    {
        switch (@event)
        {
            // case BookEvent.WritingStarted writingStarted:
            //     return new BookModel(...);

           case BookEvent.ChapterAdded chapterAdded:
               bookModel.Chapters.Add(chapterAdded.Chapter);
               return bookModel;

           case BookEvent.Published published:
               bookModel.CurrentState = BookModel.State.Published;
               return bookModel;
        }
    }
}
