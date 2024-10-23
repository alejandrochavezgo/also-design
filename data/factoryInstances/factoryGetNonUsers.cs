namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetNonUsers: baseMethod<factoryGetNonUsers, userModel>
{
    private log _logger = new log();

    protected override userModel _getEntity(IDataReader dr)
    {
        try
        {
            return new userModel
            {
                id = conversionManager.toInt(dr["USER.IDUSER"]),
                employee = new employeeModel{
                    code = conversionManager.toString(dr["EMPLOYEE.CODE"]),
                    hasUser = conversionManager.toBoolean(dr["EMPLOYEE.HASUSER"])
                },
                email = conversionManager.toString(dr["USER.EMAIL"]),
                firstname = conversionManager.toString(dr["USER.FIRSTNAME"]),
                lastname = conversionManager.toString(dr["USER.LASTNAME"]),
                status = conversionManager.toInt(dr["USER.IDSTATUS"]),
                creationDateAsString = conversionManager.toValidDate(dr["USER.CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                modificationDateAsString = conversionManager.toValidDate(dr["USER.MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["USER.MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                statusColor =  getStatusColor(conversionManager.toInt(dr["USER.IDSTATUS"])),
                statusName = getStatusName(conversionManager.toInt(dr["USER.IDSTATUS"]))
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    private string getStatusColor(int statusId)
    {
        try
        {
            var status = string.Empty;
            switch (statusId)
            {
                case 1:
                    status = "success";
                    break;
                case 2:
                    status = "danger";
                    break;
                case 3:
                    status = "dark";
                    break;
                default:
                    status = "dark";
                    break;
            }
            return status;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    private string getStatusName(int statusId)
    {
        try
        {
            var status = string.Empty;
            switch (statusId)
            {
                case 1:
                    status = "active";
                    break;
                case 2:
                    status = "inactive";
                    break;
                case 3:
                    status = "locked";
                    break;
                default:
                    status = "undefined";
                    break;
            }
            return status;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}