namespace common.constants;

public sealed class format
{
    #region Formats
    public const string FORMAT_DDMMYYYY = "dd/MM/yyyy";
    public const string FORMAT_MMDDYYYY = "MM/dd/yyyy";
    public const string FORMAT_DDMMYYYYHHMMSSTT = "dd/MM/yyyy hh:mm:ss tt";
    public const string FORMAT_MMDDYYYY_HHMMSSZZZ = "MM/dd/yyyy hh:mm:sszzz";
    public const string FORMAT_MMDDYYYY_HHMMSSFFFTT = "MM/dd/yyyy hh:mm:ss.fff tt";
    public const string FORMAT_TIMESPAN_HHMM = "hh\\:mm";
    #endregion

    #region Enums
    public enum date
    {
        ShortDate,
        MediumDate,
        LongDate,
        DateTime
    };
    #endregion Enums

    #region Date time formats
    #region Standard Date
    /// <summary>
    /// 'yyyyMMdd' Format
    /// </summary>
    public static string standardDate
    {
        get { return "yyyyMMdd"; }
    }
    #endregion

    #region shortDate
    /// <summary>
    /// 'dd/MM/yyyy' Format
    /// </summary>
    public static string shortDate
    {
        get { return "dd/MM/yyyy"; }
    }
    #endregion

    #region MediumDate
    /// <summary>
    /// 'dd-MMM-yyyy' Format
    /// </summary>
    public static string mediumDate
    {
        get { return "dd-MMM-yyyy"; }
    }
    #endregion

    #region LongDate
    /// <summary>
    /// 'd \d\e MMMM \d\e yyyy' Format
    /// </summary>
    public static string longDate
    {
        get { return @"d \d\e MMMM \d\e yyyy"; }
    }
    #endregion

    #region DateTime
    /// <summary>
    /// 'dd/MM/yyyy HH:mm:ss' Format
    /// </summary>
    public static String dateTime
    {
        get { return string.Concat(shortDate, " ", hour); }
    }
    #endregion

    #region Hour
    /// <summary>
    /// 'HH:mm:ss' Format
    /// </summary>
    public static string hour
    {
        get { return "HH:mm:ss"; }
    }
    #endregion
    #endregion Date time formats

    #region Numeric
    public static string thousand
    {
        get { return "#,##0"; }
    }

    public static string Decimal
    {
        get { return "#,##0.00"; }
    }

    public static string decimal4
    {
        get { return "#,##0.0000"; }
    }

    public static string currency
    {
        get { return "$#,##0.00"; }
    }

    public static string percentage
    {
        get { return "0.00%"; }
    }
    #endregion
}
