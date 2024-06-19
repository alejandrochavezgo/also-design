namespace data.providerData;

using System.Data;
using System.Data.Common;
using data.providerData.providers;
using data.providerData.components;

/// <summary>
/// Factory. Delega un objeto especifico del tipo DataConsumer para controlar
/// el acceso y manipulacion de la base de datos indicada.
/// Esta clase implementa el patron de diseño "Simple Factory y Factory Method" y genera un unico objeto 
/// para su uso global basado en el patron de diseño "Singleton".
/// Esta clase no es Heredable.
/// </summary>
/// <Author>Juan C. Escobar Hernández</Author>
public sealed class dataFactory
{
    private dataFactory() {; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="providerName"></param>
    /// <returns></returns>
    public static DbProviderFactory getDbProviderFactory(string providerName)
    {
        return provider.getDbFactory(providerName);
    }

    /// <summary>
    /// Metodo estatico que regresa un Parametro de Base de datos segun el proveedor indicado
    /// </summary>
    /// <param name="providerName">Cadena que indica el proveedor del DBMS a manipular</param>
    /// <param name="parameterName">Nombre del parametro de base de datos</param>
    /// <param name="dbType">Tipo de valor que toma el parametro</param>
    /// <param name="size">tamaño del parametro</param>
    /// <param name="direction">Direccion del flujo que toma el parametro</param>
    /// <returns>un objeto del tipo DBParameter segun el proveedor de BD indicado</returns>
    public static DbParameter getObjParameter(string providerName, string parameterName, DbType dbType, object Value, int size = -1, ParameterDirection direction = ParameterDirection.Input)
    {
        DbParameter param = provider.getDbFactory(providerName).CreateParameter();

        param.ParameterName = parameterName;
        param.DbType = dbType;
        if (size != -1) param.Size = size;
        param.Direction = direction;

        param.Value = Value;

        return param;
    }

    /// <summary>
    /// Metodo estatico que proporciona una unica instancia del objeto DataConsumer dado algun proveedor 
    /// indicado en la tabla del metodo estatico "GetProvidersSupport()"
    /// Este metodo implementa el patron de diseño "Factory Method" para delegar el objeto controlador
    /// de la base de datos especificada.
    /// </summary>
    /// <param name="urlConnection">Cadena de conexion a la base de datos a consumir</param>
    /// <param name="providerName">Cadena que indica el proveedor del DBMS a manipular</param>
    /// <returns>Regresa una unica instancia "Singleton" del tipo Base "DataConsumer"</returns>
    public static dataConsumer getSingletonInstance(string urlConnection, string providerName)
    {
        try
        {
            if (managerComponents.genCon == null)
            {
                managerComponents.genCon = new commonConsumer(urlConnection, providerName);
                managerComponents.olderUrlCommonConn = urlConnection;
                return managerComponents.genCon;
            }
            else if (!managerComponents.olderUrlCommonConn.Equals(urlConnection))
            {
                managerComponents.genCon = new commonConsumer(urlConnection, providerName);
                managerComponents.olderUrlCommonConn = urlConnection;
                return managerComponents.genCon;
            }
            else
                return managerComponents.genCon;
        }
        catch
        {
            throw;
        }
    }
    
    /// <summary>
    /// Metodo estatico que proporciona una nueva instancia del objeto DataConsumer dado algun proveedor 
    /// indicado en la tabla del metodo estatico "GetProvidersSupport()"
    /// </summary>
    /// <param name="urlConnection">Cadena de conexion a la base de datos a consumir</param>
    /// <param name="providerName">Cadena que indica el proveedor del DBMS a manipular</param>
    /// <returns>Regresa una nueva instancia del tipo "DataConsumer"</returns>
    public static dataConsumer getNewInstance(string urlConnection, string providerName)
    {
        try
        {
            return new commonConsumer(urlConnection, providerName);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Provee un listado con los nombre de Proveedores soportados por el Framework .NET
    /// </summary>
    /// <param name="providerName"></param> 
    /// <returns>System.Data.DataTable object</returns>
    public static System.Data.DataTable getProvidersSupport()
    {
        try
        {
            return provider.getProviders();
        }
        catch
        {
            throw;
        }
    }
}