namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetWorkOrderById: baseMethod<factoryGetWorkOrderById, workOrderModel>
{
    private log _logger = new log();

    protected override workOrderModel _getEntity(IDataReader dr)
    {
        try
        {
            return new workOrderModel
            {
                id = conversionManager.toInt(dr["IDWORKORDER"]),
                quotationId = conversionManager.toInt(dr["IDQUOTATION"]),
                priorityId = conversionManager.toInt(dr["IDPRIORITYTYPE"]),
                rfq = conversionManager.toString(dr["RFQ"]),
                deliveryDate = conversionManager.toValidDate(dr["DELIVERYDATE"]),
                deliveryDateAsString = conversionManager.toValidDate(dr["DELIVERYDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["DELIVERYDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                creationDateAsString = conversionManager.toValidDate(dr["CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                modificationDateAsString = conversionManager.toValidDate(dr["MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                status = conversionManager.toInt(dr["IDSTATUS"]),
                statusColor =  getStatusColor(conversionManager.toInt(dr["IDSTATUS"])),
                statusName = getStatusName(conversionManager.toInt(dr["IDSTATUS"]))
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