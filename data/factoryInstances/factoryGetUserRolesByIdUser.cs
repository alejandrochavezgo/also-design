using System.Data;
using common.conversions;
using entities.models;

namespace data.factoryInstances;

internal class factoryGetUserRolesByIdUser: BaseMethod<factoryGetUserRolesByIdUser, roleModel>
{
    protected override roleModel _GetEntity(IDataReader dr)
    {
        return new roleModel
        {
            id = ConversionManager.ToInt(dr["IDROLE"]),
            description = ConversionManager.ToString(dr["DESCRIPTION"])
        };
    }
}