namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetEnterpriseFullInformationByIdAndConfigType: baseMethod<factoryGetEnterpriseFullInformationByIdAndConfigType, enterpriseModel>
{
    private log _logger = new log();

    protected override enterpriseModel _getEntity(IDataReader dr)
    {
        try
        {
            return new enterpriseModel
            {
                id = conversionManager.toInt(dr["ENTERPRISE.IDENTERPRISE"]),
                city = conversionManager.toString(dr["ENTERPRISE.CITY"]),
                state = conversionManager.toString(dr["ENTERPRISE.STATE"]),
                country = conversionManager.toString(dr["ENTERPRISE.COUNTRY"]),
                quotation = new quotationModel
                {
                    notes = conversionManager.toString(dr["ENTERPRISECONFIG.TEXT"]),
                    description = conversionManager.toString(dr["ENTERPRISECONFIG.DESCRIPTION"])
                }
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}