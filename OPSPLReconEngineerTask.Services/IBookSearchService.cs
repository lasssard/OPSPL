using OPSPLReconEngineerTask.Data.Models;

namespace OPSPLReconEngineerTask.Services;

public interface IBookSearchService
{
    Task<ICollection<Book>> SearchAsync(string author, string textToFind, string currentBookHolder,
        string combineCondition, CancellationToken cancellationToken);
}