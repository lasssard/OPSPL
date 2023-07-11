using Microsoft.EntityFrameworkCore;
using OPSPLReconEngineerTask.Data;
using OPSPLReconEngineerTask.Data.DbContext;

namespace OPSPLReconEngineerTask.Services;

public class ReportBuildService : IReportBuildService
{
    private readonly OPSPLTaskContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReportBuildService(OPSPLTaskContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public async Task<IList<UserBooksHoldReport>> GetUserReportsAsync(CancellationToken cancellationToken)
    {
        var utcNow = _dateTimeProvider.GetUtcNow();
        var result = await _dbContext.BooksTakens
            .Include(x => x.User)
            .Select(x=> new
            {
                UserId = x.UserId,
                User = x.User,
                DaysTaken = x.DateTaken
            }).ToListAsync(cancellationToken);

        //Grouping on client side due to some bugs on EF Core side, groupBy with DateTime. https://github.com/dotnet/efcore/issues/22008
        return result.GroupBy(x => x.UserId, taken => taken, (r, taken) => new UserBooksHoldReport
        {
            User = taken.FirstOrDefault()!.User,
            TotalBooksHolds = taken.Count(),
            TotalDaysHolds = taken.Select(d => (utcNow - d.DaysTaken).Days).Sum()
        }).ToList();
    }
}