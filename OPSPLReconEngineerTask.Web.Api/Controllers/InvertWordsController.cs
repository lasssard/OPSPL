using Microsoft.AspNetCore.Mvc;
using OPSPLReconEngineerTask.Data.DbContext;
using OPSPLReconEngineerTask.Services;

namespace OPSPLReconEngineerTask.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvertWordsController : Controller
{
    private readonly IInvertWordsService _invertWordsService;
    private readonly OPSPLTaskContext _dataBaseContext;

    public InvertWordsController(IInvertWordsService invertWordsService, OPSPLTaskContext dataBaseContext)
    {
        _invertWordsService = invertWordsService ?? throw new ArgumentNullException(nameof(invertWordsService));
        _dataBaseContext = dataBaseContext ?? throw new ArgumentNullException(nameof(dataBaseContext));
    }
    
    
    [HttpGet(Name = "invertwords")]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken = default)
    {
        var book = await _dataBaseContext.Books.FindAsync(id, cancellationToken);
        if (book == null)
        {
            return NotFound();
        }

        var invertedTitleBook = _invertWordsService.InvertBookTitle(book);
        return Json(invertedTitleBook);
    }
}