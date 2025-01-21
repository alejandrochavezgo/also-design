namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.enums;
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
                id = conversionManager.toInt(dr["WORKORDERS.IDWORKORDER"]),
                code = conversionManager.toString(dr["WORKORDERS.CODE"]),
                userId = conversionManager.toInt(dr["WORKORDERS.IDUSER"]),
                quotationId = conversionManager.toInt(dr["WORKORDERS.IDQUOTATION"]),
                priorityId = conversionManager.toInt(dr["PRIORITYTYPES.IDPRIORITYTYPE"]),
                rfq = conversionManager.toString(dr["WORKORDERS.RFQ"]),
                deliveryDate = conversionManager.toValidDate(dr["WORKORDERS.DELIVERYDATE"]),
                deliveryDateAsString = conversionManager.toValidDate(dr["WORKORDERS.DELIVERYDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["WORKORDERS.DELIVERYDATE"]).ToString("yyyy-MM-dd") : "-",
                creationDateAsString = conversionManager.toValidDate(dr["WORKORDERS.CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                modificationDateAsString = conversionManager.toValidDate(dr["WORKORDERS.MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["WORKORDERS.MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                status = conversionManager.toInt(dr["WORKORDERS.IDSTATUS"]),
                statusColor =  getStatusColor(conversionManager.toInt(dr["WORKORDERS.IDSTATUS"])),
                statusName = getStatusName(conversionManager.toInt(dr["WORKORDERS.IDSTATUS"])),
                quotationCode = conversionManager.toString(dr["QUOTATIONS.CODE"]),
                quotationSubtotal = conversionManager.toDecimal(dr["QUOTATIONS.SUBTOTAL"]),
                quotationTax = conversionManager.toDecimal(dr["QUOTATIONS.TAX"]),
                quotationTotal = conversionManager.toDecimal(dr["QUOTATIONS.TOTAL"]),
                clientName = conversionManager.toString(dr["CLIENTS.BUSINESSNAME"]),
                projectName = conversionManager.toString(dr["PROJECTS.NAME"]),
                quotationPaymentDescription = conversionManager.toString(dr["PAYMENTTYPES.DESCRIPTION"]),
                priorityDescription = conversionManager.toString(dr["PRIORITYTYPES.DESCRIPTION"]),
                quotationCurrencyDescription = conversionManager.toString(dr["CURRENCIES.DESCRIPTION"])
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
                case (int)statusType.CANCELLED:
                    status = "danger";
                    break;
                case (int)statusType.INPROGRESS:
                    status = "info";
                    break;
                case (int)statusType.CLOSED:
                case (int)statusType.LOCKED:
                case (int)statusType.INACTIVE:
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
                case (int)statusType.LOCKED:
                    status = "locked";
                    break;
                case (int)statusType.CANCELLED:
                    status = "cancelled";
                    break;
                case (int)statusType.CLOSED:
                    status = "closed";
                    break;
                case (int)statusType.INPROGRESS:
                    status = "in-progress";
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