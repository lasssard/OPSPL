using Microsoft.AspNetCore.Mvc;
using OPSPLReconEngineerTask.Services;

namespace OPSPLReconEngineerTask.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : Controller
{
    private readonly IReportBuildService _reportBuildService;

    public ReportController(IReportBuildService reportBuildService)
    {
        _reportBuildService = reportBuildService ?? throw new ArgumentNullException(nameof(reportBuildService));
    }
    
    [HttpGet(Name = "report")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var result = await _reportBuildService.GetUserReportsAsync(cancellationToken);
        return Json(result);
    }
}