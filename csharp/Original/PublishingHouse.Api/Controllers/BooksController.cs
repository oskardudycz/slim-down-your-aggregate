using Microsoft.AspNetCore.Mvc;
using PublishingHouse.Api.Requests;
using PublishingHouse.Application.Books;
using PublishingHouse.Application.Books.Commands;
using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Entities;
using PublishingHouse.Core.Validation;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Api.Controllers;

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
    public IActionResult CreateDraft([FromBody] CreateDraftRequest request)
    {
        var bookId = Guid.NewGuid();

        var (title, author, publisherId, edition, genre) = request;

        author.AssertNotNull();

        booksService.CreateDraft(
            new CreateDraftCommand(
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
            )
        );

        return Created($"/api/books/{bookId}", bookId);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindDetailsById([FromRoute] Guid id) =>
        await booksQueryService.FindDetailsById(new BookId(id)) is { } result ? Ok(result) : NotFound();
}
