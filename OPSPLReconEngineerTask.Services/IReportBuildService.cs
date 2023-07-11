using OPSPLReconEngineerTask.Data;

namespace OPSPLReconEngineerTask.Services;

public interface IReportBuildService
{
    Task<IList<UserBooksHoldReport>> GetUserReportsAsync(CancellationToken cancellationToken);
}