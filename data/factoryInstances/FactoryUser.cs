using System.Data;
using common.conversions;
using entities.models;

namespace data.factoryInstances;

internal class FactoryGetUserByIdUser: BaseMethod<FactoryGetUserByIdUser, UserModel> {
    protected override UserModel _GetEntity(IDataReader dr)
    {
        return new UserModel {
            id = ConversionManager.ToInt(dr["IDUSER"]),
            username = ConversionManager.ToString(dr["USERNAME"])
        };
    }
}