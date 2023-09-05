using Microsoft.AspNetCore.Mvc;
using PublishingHouse.Api.Requests;
using PublishingHouse.Application.Books;
using PublishingHouse.Application.Books.Commands;

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
    public IActionResult CreateDraft([FromBody] CreateDraftRequest? request)
    {
        var bookId = Guid.NewGuid();

        booksService.CreateDraft(new CreateDraftCommand(bookId));

        return Created($"/api/books/{bookId}", bookId);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetailsById([FromRoute] Guid id) =>
        await booksQueryService.GetDetailsById(id) is {} result ? Ok(result): NotFound();
}
