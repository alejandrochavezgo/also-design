using Microsoft.Extensions.Configuration;

namespace common.configurations
{
    public static class ConfigurationManager
    {
        public static IConfiguration AppSettings { get; }
        static ConfigurationManager()
        {
            AppSettings = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                    .Build();
        }

        public static string StringConectionDB
        {
            get
            {
                try
                {
                    return AppSettings["ConnectionStrings:cbsdbConnection"];
                }
                catch (NullReferenceException)
                {
                    throw new Exception("Database Connection String not defined in configuration file.");
                }
            }
        }

        public static string ProviderDB
        {
            get
            {
                try
                {
                    return AppSettings["providers:cbsdbProviderName"];
                }
                catch (NullReferenceException)
                {
                    throw new Exception("Database Provider not defined in configuration file.");
                }
            }
        }

        public static string StringConectionDB_ReadOnly
        {
            get
            {
                try
                {
                    return AppSettings["ConnectionStrings:cbsdbConnection_readonly"];
                }
                catch (NullReferenceException)
                {
                    throw new Exception("Database Connection String not defined in configuration file.");
                }
            }
        }

        public static string ProviderDB_ReadOnly
        {
            get
            {
                try
                {
                    return AppSettings["providers:cbsdbProviderName"];
                }
                catch (NullReferenceException)
                {
                    throw new Exception("Database Provider not defined in configuration file.");
                }
            }
        }
    }
}