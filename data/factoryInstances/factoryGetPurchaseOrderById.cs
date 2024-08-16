namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetPurchaseOrderById: baseMethod<factoryGetPurchaseOrderById, purchaseOrderModel>
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
                taxRate = conversionManager.toDecimal(dr["PURCHASEORDERS.TAXRATE"]),
                totalAmount = conversionManager.toDecimal(dr["PURCHASEORDERS.TOTAL"]),
                creationDate = conversionManager.toValidDate(dr["PURCHASEORDERS.CREATIONDATE"]),
                creationDateAsString = conversionManager.toValidDate(dr["PURCHASEORDERS.CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                modificationDateAsString = conversionManager.toValidDate(dr["PURCHASEORDERS.MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["PURCHASEORDERS.MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                status = conversionManager.toInt(dr["PURCHASEORDERS.IDSTATUS"]),
                statusColor =  getStatusColor(conversionManager.toInt(dr["PURCHASEORDERS.IDSTATUS"])),
                statusName = getStatusName(conversionManager.toInt(dr["PURCHASEORDERS.IDSTATUS"])),
                generalNotes = conversionManager.toString(dr["PURCHASEORDERS.GENERALNOTES"]),
                supplier = new supplierModel
                {
                    id = conversionManager.toInt(dr["SUPPLIERS.IDSUPPLIER"]),
                    businessName = conversionManager.toString(dr["SUPPLIERS.BUSINESSNAME"]),
                    address = conversionManager.toString(dr["SUPPLIERS.ADDRESS"]),
                    city = conversionManager.toString(dr["SUPPLIERS.CITY"]),
                    rfc = conversionManager.toString(dr["SUPPLIERS.RFC"]),
                    mainContactName = conversionManager.toString(dr["SUPPLIERS.SUPPLIERCONTACTNAME"]),
                    mainContactPhone = conversionManager.toString(dr["SUPPLIERS.SUPPLIERCONTACTPHONE"]),
                    status = conversionManager.toInt(dr["SUPPLIERS.IDSTATUS"]),
                    statusColor =  getStatusColor(conversionManager.toInt(dr["SUPPLIERS.IDSTATUS"])),
                    statusName = getStatusName(conversionManager.toInt(dr["SUPPLIERS.IDSTATUS"]))
                },
                user = new userModel
                {
                    id = conversionManager.toInt(dr["USERS.IDUSER"]),
                    username = conversionManager.toString(dr["USERS.USERNAME"]),
                    employee = new employeeModel
                    {
                        id = conversionManager.toInt(dr["EMPLOYEES.IDEMPLOYEE"]),
                        mainContactPhone = conversionManager.toString(dr["USERS.USERCONTACTPHONE"]),
                    },
                    status = conversionManager.toInt(dr["USERS.IDSTATUS"]),
                    statusColor =  getStatusColor(conversionManager.toInt(dr["USERS.IDSTATUS"])),
                    statusName = getStatusName(conversionManager.toInt(dr["USERS.IDSTATUS"])),
                },
                payment = new paymentModel
                {
                    description = conversionManager.toString(dr["PAYMENTTYPES.DESCRIPTION"]),
                    isActive = conversionManager.toBoolean(dr["PAYMENTTYPES.IDSTATUS"])
                },
                currency = new currencyModel
                {
                    description = conversionManager.toString(dr["CURRENCIES.DESCRIPTION"]),
                    isActive = conversionManager.toBoolean(dr["CURRENCIES.IDSTATUS"])
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
                case 1:
                    status = "success";
                    break;
                case 2:
                    status = "danger";
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