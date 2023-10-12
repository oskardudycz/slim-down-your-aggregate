using Microsoft.AspNetCore.Mvc;
using PublishingHouse.Api.Requests;
using PublishingHouse.Application.Books;
using PublishingHouse.Books;
using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Draft;
using PublishingHouse.Books.Entities;
using PublishingHouse.Core.Validation;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Api.Controllers;

using static DraftCommand;

[Route("api/[controller]")]
public class BooksController: Controller
{
    private readonly IBooksService booksService;
    private readonly IBookQueryService booksQueryService;

    public BooksController(IBooksService booksService, IBookQueryService booksQueryService)
    {
        this.booksService = booksService;
        this.booksQueryService = booksQueryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDraft([FromBody] CreateDraftRequest request, CancellationToken ct)
    {
        var bookId = Guid.NewGuid();

        var (title, author, publisherId, edition, genre) = request;

        author.AssertNotNull();

        await booksService.CreateDraft(
            new CreateDraft(
                new BookId(bookId),
                new Title(title.AssertNotEmpty()),
                new AuthorIdOrData(
                    author.AuthorId.HasValue ? new AuthorId(author.AuthorId.Value) : null,
                    author.FirstName != null ? new AuthorFirstName(author.FirstName) : null,
                    author.LastName != null ? new AuthorLastName(author.LastName) : null
                ),
                new PublisherId(publisherId.AssertNotEmpty()),
                new PositiveInt(edition.GetValueOrDefault()),
                genre != null ? new Genre(genre) : null
            ),
            ct
        );

        return Created($"/api/books/{bookId}", bookId);
    }

    [HttpPost("{id}/chapters")]
    public async Task<IActionResult> AddChapter([FromRoute] Guid id, [FromBody] AddChapterRequest request, CancellationToken ct)
    {
        var (title, content) = request;

        await booksService.AddChapter(
            new AddChapter(
                new BookId(id),
                new ChapterTitle(title.AssertNotEmpty()),
                content != null ? new ChapterContent(content): ChapterContent.Empty
            ),
            ct
        );

        return NoContent();
    }

    [HttpPost("{id}/move-to-editing")]
    public async Task<IActionResult> MoveToEditing([FromRoute] Guid id, CancellationToken ct)
    {
        await booksService.MoveToEditing(
            new MoveToEditing(new BookId(id)),
            ct
        );

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindDetailsById([FromRoute] Guid id, CancellationToken ct) =>
        await booksQueryService.FindDetailsById(new BookId(id), ct) is { } result ? Ok(result) : NotFound();
}
