namespace common.configurations;

using Microsoft.Extensions.Configuration;

public static class configurationManager
{
    public static IConfiguration appSettings { get; }

    static configurationManager()
    {
        appSettings = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                .Build();
    }

    public static string stringConectionDB
    {
        get
        {
            try
            {
                return appSettings["connectionStrings:alsoConnection"];
            }
            catch (NullReferenceException)
            {
                throw new Exception("Database Connection String not defined in configuration file.");
            }
        }
    }

    public static string providerDB
    {
        get
        {
            try
            {
                return appSettings["providers:alsoProviderName"];
            }
            catch (NullReferenceException)
            {
                throw new Exception("Database Provider not defined in configuration file.");
            }
        }
    }

    public static string stringConectionDB_ReadOnly
    {
        get
        {
            try
            {
                return appSettings["connectionStrings:alsoConnection_readonly"];
            }
            catch (NullReferenceException)
            {
                throw new Exception("Database Connection String not defined in configuration file.");
            }
        }
    }

    public static string providerDB_ReadOnly
    {
        get
        {
            try
            {
                return appSettings["providers:alsoProviderName"];
            }
            catch (NullReferenceException)
            {
                throw new Exception("Database Provider not defined in configuration file.");
            }
        }
    }
}