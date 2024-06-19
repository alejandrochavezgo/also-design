namespace data.factoryInstances;

using System.Data;
using common.conversions;
using entities.models;

internal class factoryGetUserRolesByIdUser: baseMethod<factoryGetUserRolesByIdUser, roleModel>
{
    protected override roleModel _getEntity(IDataReader dr)
    {
        return new roleModel
        {
            id = conversionManager.toInt(dr["IDROLE"]),
            description = conversionManager.toString(dr["DESCRIPTION"])
        };
    }
}