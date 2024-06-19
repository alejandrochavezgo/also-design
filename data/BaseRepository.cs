namespace data;

using data.providerData;
using common.configurations;

public class baseRepository
{
    protected dataConsumer _providerDB;
    protected dataConsumer _providerDB_ReadOnly;
    protected baseRepository()
    {
        _providerDB = dataFactory.getNewInstance(
                                configurationManager.stringConectionDB,
                                configurationManager.providerDB);
        _providerDB.autoOpenAndCloseConnectionForDataReader = true;


        _providerDB_ReadOnly = dataFactory.getNewInstance(
                                configurationManager.stringConectionDB_ReadOnly,
                                configurationManager.providerDB_ReadOnly);
        _providerDB_ReadOnly.autoOpenAndCloseConnectionForDataReader = true;
    }
}