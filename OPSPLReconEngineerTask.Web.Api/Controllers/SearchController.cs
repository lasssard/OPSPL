using Microsoft.AspNetCore.Mvc;
using OPSPLReconEngineerTask.Services;

namespace OPSPLReconEngineerTask.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : Controller
{
    private readonly IBookSearchService _bookSearchService;

    public SearchController(IBookSearchService bookSearchService)
    {
        _bookSearchService = bookSearchService;
    }
    
    [HttpGet(Name = "search")]
    public async Task<IActionResult> Get(string author = "", string bookText = "", string currentBookHolder = "", string combineCondition = "OR", CancellationToken cancellationToken = default)
    {
        var books = await _bookSearchService.SearchAsync(author, bookText, currentBookHolder, combineCondition, cancellationToken);
        return Json(books);
    }
}