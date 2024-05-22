using data.providerData;
using common.configurations;

namespace data
{
    public class BaseRepository
    {
        protected DataConsumer _ProviderDB;
        protected DataConsumer _ProviderDB_ReadOnly;
        protected BaseRepository()
        {
            _ProviderDB = DataFactory.GetNewInstance(
                                    ConfigurationManager.StringConectionDB,
                                    ConfigurationManager.ProviderDB);
            _ProviderDB.AutoOpenAndCloseConnectionForDataReader = true;


            _ProviderDB_ReadOnly = DataFactory.GetNewInstance(
                                   ConfigurationManager.StringConectionDB_ReadOnly,
                                   ConfigurationManager.ProviderDB_ReadOnly);
            _ProviderDB_ReadOnly.AutoOpenAndCloseConnectionForDataReader = true;
        }
    }
}