namespace common.helpers;

using common.logging;

public class dateHelper
{
    private log _logger;

    public dateHelper ()
    {
        _logger = new log();
    }

    public DateTime pstNow()
    {
        try
        {
            var utcNow = DateTime.UtcNow;
            var pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, pstZone);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}