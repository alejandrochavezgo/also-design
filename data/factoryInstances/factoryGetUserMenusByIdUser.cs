using System.Data;
using common.conversions;
using entities.models;

namespace data.factoryInstances;

internal class factoryGetUserMenusByIdUser: BaseMethod<factoryGetUserMenusByIdUser, menuModel>
{
    protected override menuModel _GetEntity(IDataReader dr)
    {
        return new menuModel
        {
            id = ConversionManager.ToInt(dr["IDMENU"]),
            description = ConversionManager.ToString(dr["DESCRIPTION"]),
            path = ConversionManager.ToString(dr["PATH"])
        };
    }
}