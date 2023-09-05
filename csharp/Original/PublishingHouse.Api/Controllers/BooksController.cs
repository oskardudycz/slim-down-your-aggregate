using Microsoft.AspNetCore.Mvc;

namespace PublishingHouse.Api.Controllers;

[Route("api/[controller]")]
public class BooksController: Controller
{
    [HttpPost]
    public IActionResult CreateDraft()//[FromBody] OpenShoppingCartRequest? request)
    {
        return Created("/api/books/", Guid.Empty); //{cartId}", cartId);
    }
}
