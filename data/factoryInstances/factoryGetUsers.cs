namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetUsers: baseMethod<factoryGetUsers, userModel>
{
    private log _logger = new log();

    protected override userModel _getEntity(IDataReader dr)
    {
        try
        {
            return new userModel
            {
                id = conversionManager.toInt(dr["IDUSER"]),
                username = conversionManager.toString(dr["USERNAME"]),
                email = conversionManager.toString(dr["EMAIL"]),
                firstname = conversionManager.toString(dr["FIRSTNAME"]),
                lastname = conversionManager.toString(dr["LASTNAME"]),
                isActive = conversionManager.toBoolean(dr["ISACTIVE"]),
                isLocked = conversionManager.toBoolean(dr["ISLOCKED"]),
                creationDateAsString = conversionManager.toValidDate(dr["CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                modificationDateAsString = conversionManager.toValidDate(dr["MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
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