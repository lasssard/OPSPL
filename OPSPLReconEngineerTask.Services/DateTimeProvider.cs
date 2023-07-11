namespace OPSPLReconEngineerTask.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }

    public DateTime GetNow()
    {
        return DateTime.Now;
    }
}