namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetProjects: baseMethod<factoryGetProjects, projectModel>
{
    private log _logger = new log();

    protected override projectModel _getEntity(IDataReader dr)
    {
        try
        {
            return new projectModel
            {
                id = conversionManager.toInt(dr["PROJECTS.IDPROJECT"]),
                name = conversionManager.toString(dr["PROJECTS.NAME"]),
                description = conversionManager.toString(dr["PROJECTS.DESCRIPTION"]),
                client = new clientModel ()
                {
                    id = conversionManager.toInt(dr["CLIENTS.IDCLIENT"]),
                    businessName = conversionManager.toString(dr["CLIENTS.BUSINESSNAME"])
                },
                creationDateAsString = conversionManager.toValidDate(dr["PROJECTS.CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                startDateAsString = conversionManager.toValidDate(dr["PROJECTS.STARTDATE"]).ToString("yyyy-MM-dd"),
                endDateAsString = conversionManager.toValidDate(dr["PROJECTS.ENDDATE"]).ToString("yyyy-MM-dd"),
                startDate = conversionManager.toValidDate(dr["PROJECTS.STARTDATE"]),
                endDate = conversionManager.toValidDate(dr["PROJECTS.ENDDATE"]),
                modificationDateAsString = conversionManager.toValidDate(dr["PROJECTS.MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["PROJECTS.MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                status = conversionManager.toInt(dr["PROJECTS.IDSTATUS"]),
                statusColor =  getStatusColor(conversionManager.toInt(dr["PROJECTS.IDSTATUS"])),
                statusName = getStatusName(conversionManager.toInt(dr["PROJECTS.IDSTATUS"]))
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
                case (int)statusType.ACTIVE:
                    status = "success";
                    break;
                case (int)statusType.INACTIVE:
                case (int)statusType.CANCELLED:
                    status = "danger";
                    break;
                case (int)statusType.LOCKED:
                case (int)statusType.CLOSED:
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
                case (int)statusType.ACTIVE:
                    status = "active";
                    break;
                case (int)statusType.INACTIVE:
                    status = "inactive";
                    break;
                case (int)statusType.CANCELLED:
                    status = "cancelled";
                    break;
                case (int)statusType.LOCKED:
                    status = "locked";
                    break;
                case (int)statusType.CLOSED:
                    status = "closed";
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