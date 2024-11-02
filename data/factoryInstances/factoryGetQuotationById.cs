namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetQuotationById: baseMethod<factoryGetQuotationById, quotationModel>
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
                taxRate = conversionManager.toDecimal(dr["QUOTATIONS.TAXRATE"]),
                totalAmount = conversionManager.toDecimal(dr["QUOTATIONS.TOTAL"]),
                creationDate = conversionManager.toValidDate(dr["QUOTATIONS.CREATIONDATE"]),
                creationDateAsString = conversionManager.toValidDate(dr["QUOTATIONS.CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss"),
                modificationDateAsString = conversionManager.toValidDate(dr["QUOTATIONS.MODIFICATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["QUOTATIONS.MODIFICATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
                status = conversionManager.toInt(dr["QUOTATIONS.IDSTATUS"]),
                statusColor =  getStatusColor(conversionManager.toInt(dr["QUOTATIONS.IDSTATUS"])),
                statusName = getStatusName(conversionManager.toInt(dr["QUOTATIONS.IDSTATUS"])),
                generalNotes = conversionManager.toString(dr["QUOTATIONS.GENERALNOTES"]),
                client = new clientModel
                {
                    id = conversionManager.toInt(dr["CLIENTS.IDCLIENT"]),
                    businessName = conversionManager.toString(dr["CLIENTS.BUSINESSNAME"]),
                    address = conversionManager.toString(dr["CLIENTS.ADDRESS"]),
                    city = conversionManager.toString(dr["CLIENTS.CITY"]),
                    rfc = conversionManager.toString(dr["CLIENTS.RFC"]),
                    mainContactName = conversionManager.toString(dr["CLIENTS.CLIENTCONTACTNAME"]),
                    mainContactPhone = conversionManager.toString(dr["CLIENTS.CLIENTCONTACTPHONE"]),
                    status = conversionManager.toInt(dr["CLIENTS.IDSTATUS"]),
                    statusColor =  getStatusColor(conversionManager.toInt(dr["CLIENTS.IDSTATUS"])),
                    statusName = getStatusName(conversionManager.toInt(dr["CLIENTS.IDSTATUS"]))
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
                    id = conversionManager.toInt(dr["PAYMENTTYPES.IDPAYMENTTYPE"]),
                    description = conversionManager.toString(dr["PAYMENTTYPES.DESCRIPTION"]),
                    isActive = conversionManager.toBoolean(dr["PAYMENTTYPES.IDSTATUS"])
                },
                currency = new currencyModel
                {
                    id = conversionManager.toInt(dr["CURRENCIES.IDCURRENCY"]),
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