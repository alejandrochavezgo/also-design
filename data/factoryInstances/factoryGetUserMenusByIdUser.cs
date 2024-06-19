namespace data.factoryInstances;

using System.Data;
using common.conversions;
using entities.models;

internal class factoryGetUserMenusByIdUser: baseMethod<factoryGetUserMenusByIdUser, menuModel>
{
    protected override menuModel _getEntity(IDataReader dr)
    {
        return new menuModel
        {
            id = conversionManager.toInt(dr["IDMENU"]),
            description = conversionManager.toString(dr["DESCRIPTION"]),
            path = conversionManager.toString(dr["PATH"])
        };
    }
}