namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetItemInventoryById: baseMethod<factoryGetItemInventoryById, inventoryItemModel>
{
    private log _logger = new log();

    protected override inventoryItemModel _getEntity(IDataReader dr)
    {
        try
        {
            return new inventoryItemModel
            {
                id = conversionManager.toInt(dr["IDINVENTORY"]),
                itemCode = conversionManager.toString(dr["CODE"]),
                itemName = conversionManager.toString(dr["NAME"]),
                status = conversionManager.toInt(dr["IDSTATUS"]),
                statusColor =  getStatusColor(conversionManager.toInt(dr["IDSTATUS"])),
                statusName = getStatusName(conversionManager.toInt(dr["IDSTATUS"])),
                description = conversionManager.toString(dr["DESCRIPTION"]),
                material = conversionManager.toInt(dr["IDMATERIALTYPE"]),
                materialDescription = conversionManager.toString(dr["MATERIALTYPES.DESCRIPTION"]),
                finishType = conversionManager.toInt(dr["IDFINISHTYPE"]),
                finishTypeDescription = conversionManager.toString(dr["FINISHTYPES.DESCRIPTION"]),
                classificationType = conversionManager.toInt(dr["CLASSIFICATIONTYPE.IDCLASSIFICATIONTYPE"]),
                classificationTypeDescription = conversionManager.toString(dr["CLASSIFICATIONTYPE.DESCRIPTION"]),
                diameter = conversionManager.toDouble(dr["DIAMETER"]),
                unitDiameter = conversionManager.toInt(dr["IDDIAMETERUNITTYPE"]),
                unitDiameterDescription = conversionManager.toString(dr["DIAMETERLENGTHUNITTYPES.DESCRIPTION"]),
                length = conversionManager.toDouble(dr["LENGTH"]),
                unitLength = conversionManager.toInt(dr["IDLENGTHUNITTYPE"]),
                unitLengthDescription = conversionManager.toString(dr["LENGTHUNITTYPES.DESCRIPTION"]),
                weight = conversionManager.toDouble(dr["WEIGHT"]),
                unitWeight = conversionManager.toInt(dr["IDWEIGHTUNITTYPE"]),
                unitWeightDescription = conversionManager.toString(dr["WEIGHTUNITTYPES.DESCRIPTION"]),
                tolerance = conversionManager.toDouble(dr["TOLERANCE"]),
                unitTolerance = conversionManager.toInt(dr["IDTOLERANCEUNITTYPE"]),
                unitToleranceDescription = conversionManager.toString(dr["TOLERANCEUNITTYPES.DESCRIPTION"]),
                warehouseLocation = conversionManager.toInt(dr["IDWAREHOUSELOCATIONTYPE"]),
                warehouseLocationDescription = conversionManager.toString(dr["WAREHOUSELOCATIONTYPES.DESCRIPTION"]),
                reorderQty = conversionManager.toDouble(dr["REORDERQUANTITY"]),
                notes = conversionManager.toString(dr["NOTES"]),
                itemImagePath = conversionManager.toString(dr["ITEMIMAGEPATH"]),
                bluePrintsPath = conversionManager.toString(dr["BLUEPRINTSPATH"]),
                technicalSpecificationsPath = conversionManager.toString(dr["TECHNICALSPECIFICATIONSPATH"])
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