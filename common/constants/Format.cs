namespace common.constants
{
    public sealed class Format
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
        public enum Date
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
        public static String StandardDate
        {
            get { return "yyyyMMdd"; }
        }
        #endregion

        #region ShortDate
        /// <summary>
        /// 'dd/MM/yyyy' Format
        /// </summary>
        public static String ShortDate
        {
            get { return "dd/MM/yyyy"; }
        }
        #endregion

        #region MediumDate
        /// <summary>
        /// 'dd-MMM-yyyy' Format
        /// </summary>
        public static String MediumDate
        {
            get { return "dd-MMM-yyyy"; }
        }
        #endregion

        #region LongDate
        /// <summary>
        /// 'd \d\e MMMM \d\e yyyy' Format
        /// </summary>
        public static String LongDate
        {
            get { return @"d \d\e MMMM \d\e yyyy"; }
        }
        #endregion

        #region DateTime
        /// <summary>
        /// 'dd/MM/yyyy HH:mm:ss' Format
        /// </summary>
        public static String DateTime
        {
            get { return String.Concat(ShortDate, " ", Hour); }
        }
        #endregion

        #region Hour
        /// <summary>
        /// 'HH:mm:ss' Format
        /// </summary>
        public static String Hour
        {
            get { return "HH:mm:ss"; }
        }
        #endregion
        #endregion Date time formats

        #region Numeric
        public static String Thousand
        {
            get { return "#,##0"; }
        }


        public static String Decimal
        {
            get { return "#,##0.00"; }
        }

        public static String Decimal4
        {
            get { return "#,##0.0000"; }
        }

        public static String Currency
        {
            get { return "$#,##0.00"; }
        }

        public static String Percentage
        {
            get { return "0.00%"; }
        }
        #endregion
    }
}