namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetQuotationsByTerm: baseMethod<factoryGetQuotationsByTerm, quotationModel>
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
                client = new clientModel
                {
                    id = conversionManager.toInt(dr["CLIENTS.IDCLIENT"]),
                    businessName = conversionManager.toString(dr["CLIENTS.BUSINESSNAME"])
                },
                payment = new paymentModel
                {
                    description = conversionManager.toString(dr["PAYMENTTYPES.DESCRIPTION"])
                },
                currency = new currencyModel
                {
                    description = conversionManager.toString(dr["CURRENCYTYPES.DESCRIPTION"])
                },
                subtotal = conversionManager.toDecimal(dr["QUOTATIONS.SUBTOTAL"]),
                taxAmount = conversionManager.toDecimal(dr["QUOTATIONS.TAX"]),
                totalAmount = conversionManager.toDecimal(dr["QUOTATIONS.TOTAL"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}