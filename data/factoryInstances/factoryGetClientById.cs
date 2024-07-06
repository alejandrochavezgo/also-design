namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetClientById: baseMethod<factoryGetClientById, clientModel>
{
    private log _logger = new log();

    protected override clientModel _getEntity(IDataReader dr)
    {
        try
        {
            return new clientModel
            {
                id = conversionManager.toInt(dr["IDCLIENT"]),
                businessName = conversionManager.toString(dr["BUSINESSNAME"]),
                rfc = conversionManager.toString(dr["RFC"]),
                address = conversionManager.toString(dr["ADDRESS"]),
                zipCode = conversionManager.toString(dr["ZIPCODE"]),
                city = conversionManager.toString(dr["CITY"]),
                state = conversionManager.toString(dr["STATE"]),
                country = conversionManager.toString(dr["COUNTRY"]),
                creationDateAsString = conversionManager.toValidDate(dr["CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                modificationDateAsString = conversionManager.toValidDate(dr["MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                isActive = conversionManager.toBoolean(dr["ISACTIVE"]),
                statusColor = conversionManager.toBoolean(dr["ISACTIVE"]) ? "success" : "danger",
                statusName = conversionManager.toBoolean(dr["ISACTIVE"]) ? "Active" : "Inactive",
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}