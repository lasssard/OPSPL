namespace OPSPLReconEngineerTask.Services;

public interface IDateTimeProvider
{
    DateTime GetUtcNow();
    DateTime GetNow();
}