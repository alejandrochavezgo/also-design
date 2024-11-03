namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetQuotations: baseMethod<factoryGetQuotations, quotationModel>
{
    private log _logger = new log();

    protected override quotationModel _getEntity(IDataReader dr)
    {
        try
        {
            return new quotationModel
            {
                id = conversionManager.toInt(dr["QUOTATIONS.IDQUOTATION"]),
                code = conversionManager.toString(dr["QUOTATIONS.CODE"]),
                subtotal = conversionManager.toDecimal(dr["QUOTATIONS.SUBTOTAL"]),
                taxAmount = conversionManager.toDecimal(dr["QUOTATIONS.TAX"]),
                totalAmount = conversionManager.toDecimal(dr["QUOTATIONS.TOTAL"]),
                creationDate = conversionManager.toValidDate(dr["QUOTATIONS.CREATIONDATE"]),
                creationDateAsString = conversionManager.toValidDate(dr["QUOTATIONS.CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                modificationDateAsString = conversionManager.toValidDate(dr["QUOTATIONS.MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["QUOTATIONS.MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                status = conversionManager.toInt(dr["QUOTATIONS.IDSTATUS"]),
                statusColor =  getStatusColor(conversionManager.toInt(dr["QUOTATIONS.IDSTATUS"])),
                statusName = getStatusName(conversionManager.toInt(dr["QUOTATIONS.IDSTATUS"])),
                client = new clientModel
                {
                    id = conversionManager.toInt(dr["CLIENTS.IDCLIENT"]),
                    businessName = conversionManager.toString(dr["CLIENTS.BUSINESSNAME"])
                },
                user = new userModel
                {
                    id = conversionManager.toInt(dr["USERS.IDUSER"]),
                    username = conversionManager.toString(dr["USERS.USERNAME"])
                },
                payment = new paymentModel
                {
                    description = conversionManager.toString(dr["PAYMENTTYPES.DESCRIPTION"])
                },
                currency = new currencyModel
                {
                    description = conversionManager.toString(dr["CURRENCIES.DESCRIPTION"])
                }
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
                    status = "primary";
                    break;
                case (int)statusType.PENDING:
                    status = "warning";
                    break;
                case (int)statusType.LINKED:
                    status = "success";
                    break;
                case (int)statusType.APPROVED:
                    status = "secondary";
                    break;
                case (int)statusType.REJECTED:
                case (int)statusType.CANCELLED:
                case (int)statusType.EXPIRED:
                    status = "danger";
                    break;
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
                case (int)statusType.PENDING:
                    status = "pending";
                    break;
                case (int)statusType.LINKED:
                    status = "linked";
                    break;
                case (int)statusType.APPROVED:
                    status = "approved";
                    break;
                case (int)statusType.REJECTED:
                    status = "rejected";
                    break;
                case (int)statusType.CANCELLED:
                    status = "cancelled";
                    break;
                case (int)statusType.CLOSED:
                    status = "closed";
                    break;
                case (int)statusType.EXPIRED:
                    status = "expired";
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