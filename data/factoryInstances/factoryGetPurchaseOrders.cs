namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetPurchaseOrders: baseMethod<factoryGetPurchaseOrders, purchaseOrderModel>
{
    private log _logger = new log();

    protected override purchaseOrderModel _getEntity(IDataReader dr)
    {
        try
        {
            return new purchaseOrderModel
            {
                id = conversionManager.toInt(dr["PURCHASEORDERS.IDPURCHASEORDER"]),
                code = conversionManager.toString(dr["PURCHASEORDERS.CODE"]),
                subtotal = conversionManager.toDecimal(dr["PURCHASEORDERS.SUBTOTAL"]),
                taxAmount = conversionManager.toDecimal(dr["PURCHASEORDERS.TAX"]),
                totalAmount = conversionManager.toDecimal(dr["PURCHASEORDERS.TOTAL"]),
                creationDate = conversionManager.toValidDate(dr["PURCHASEORDERS.CREATIONDATE"]),
                creationDateAsString = conversionManager.toValidDate(dr["PURCHASEORDERS.CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                modificationDateAsString = conversionManager.toValidDate(dr["PURCHASEORDERS.MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["PURCHASEORDERS.MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                status = conversionManager.toInt(dr["PURCHASEORDERS.IDSTATUS"]),
                statusColor =  getStatusColor(conversionManager.toInt(dr["PURCHASEORDERS.IDSTATUS"])),
                statusName = getStatusName(conversionManager.toInt(dr["PURCHASEORDERS.IDSTATUS"])),
                supplier = new supplierModel
                {
                    id = conversionManager.toInt(dr["SUPPLIERS.IDSUPPLIER"]),
                    businessName = conversionManager.toString(dr["SUPPLIERS.BUSINESSNAME"])
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
                case (int)statusType.APPROVED:
                    status = "secondary";
                    break;
                case (int)statusType.PARTIALLY_FULFILLED:
                    status = "info";
                    break;
                case (int)statusType.FULFILLED:
                    status = "success";
                    break;
                case (int)statusType.REJECTED:
                case (int)statusType.CANCELLED:
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
                case (int)statusType.APPROVED:
                    status = "approved";
                    break;
                case (int)statusType.PARTIALLY_FULFILLED:
                    status = "partially fulfilled";
                    break;
                case (int)statusType.FULFILLED:
                    status = "fulfilled";
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