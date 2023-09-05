using Microsoft.AspNetCore.Mvc;
using PublishingHouse.Api.Requests;
using PublishingHouse.Application.Books;
using PublishingHouse.Application.Books.Commands;

namespace PublishingHouse.Api.Controllers;

[Route("api/[controller]")]
public class BooksController: Controller
{
    private readonly IBooksService booksService;

    public BooksController(IBooksService booksService) =>
        this.booksService = booksService;

    [HttpPost]
    public IActionResult CreateDraft([FromBody] CreateDraftRequest? request)
    {
        var bookId = Guid.NewGuid();

        booksService.CreateDraft(new CreateDraftCommand(bookId));

        return Created($"/api/books/{bookId}", bookId);
    }
}
