namespace app.helpers;

public static class dateHelper
{
    public static DateTime pstNow()
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