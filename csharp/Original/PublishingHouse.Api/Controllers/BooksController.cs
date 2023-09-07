using Microsoft.AspNetCore.Mvc;
using PublishingHouse.Api.Requests;
using PublishingHouse.Application.Books;
using PublishingHouse.Application.Books.Commands;
using PublishingHouse.Books.Entities;
using PublishingHouse.Core.Validation;

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

        var (title, author, publisher, edition, genre) = request;

        booksService.CreateDraft(
            new CreateDraftCommand(
                new BookId(bookId),
                new Title(title.AssertNotEmpty()),
                new Author(
                    author.AssertNotNull().FirstName.AssertNotEmpty(),
                    author.LastName.AssertNotEmpty()
                ),
                new Publisher(publisher.AssertNotEmpty()),
                edition.AssertNotEmpty(),
                genre != null ? new Genre(genre): null
            )
        );

        return Created($"/api/books/{bookId}", bookId);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindDetailsById([FromRoute] Guid id) =>
        await booksQueryService.FindDetailsById(new BookId(id)) is { } result ? Ok(result) : NotFound();
}
