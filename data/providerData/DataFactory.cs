using System.Data;
using System.Data.Common;
using data.providerData.providers;
using data.providerData.components;

namespace data.providerData
{
    /// <summary>
    /// Factory. Delega un objeto especifico del tipo DataConsumer para controlar
    /// el acceso y manipulacion de la base de datos indicada.
    /// Esta clase implementa el patron de diseño "Simple Factory y Factory Method" y genera un unico objeto 
    /// para su uso global basado en el patron de diseño "Singleton".
    /// Esta clase no es Heredable.
    /// </summary>
    /// <Author>Juan C. Escobar Hernández</Author>
    public sealed class DataFactory
    {
        private DataFactory() {; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProviderName"></param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(string ProviderName)
        {
            return Provider.GetDbFactory(ProviderName);
        }

        /// <summary>
        /// Metodo estatico que regresa un Parametro de Base de datos segun el proveedor indicado
        /// </summary>
        /// <param name="ProviderName">Cadena que indica el proveedor del DBMS a manipular</param>
        /// <param name="ParameterName">Nombre del parametro de base de datos</param>
        /// <param name="dbType">Tipo de valor que toma el parametro</param>
        /// <param name="Size">tamaño del parametro</param>
        /// <param name="Direction">Direccion del flujo que toma el parametro</param>
        /// <returns>un objeto del tipo DBParameter segun el proveedor de BD indicado</returns>
        public static DbParameter GetObjParameter(string ProviderName, string ParameterName, DbType dbType, object Value, int Size = -1, ParameterDirection Direction = ParameterDirection.Input)
        {
            DbParameter param = Provider.GetDbFactory(ProviderName).CreateParameter();

            param.ParameterName = ParameterName;
            param.DbType = dbType;
            if (Size != -1) param.Size = Size;
            param.Direction = Direction;

            param.Value = Value;

            return param;
        }

        /// <summary>
        /// Metodo estatico que proporciona una unica instancia del objeto DataConsumer dado algun proveedor 
        /// indicado en la tabla del metodo estatico "GetProvidersSupport()"
        /// Este metodo implementa el patron de diseño "Factory Method" para delegar el objeto controlador
        /// de la base de datos especificada.
        /// </summary>
        /// <param name="UrlConnection">Cadena de conexion a la base de datos a consumir</param>
        /// <param name="ProviderName">Cadena que indica el proveedor del DBMS a manipular</param>
        /// <returns>Regresa una unica instancia "Singleton" del tipo Base "DataConsumer"</returns>
        public static DataConsumer GetSingletonInstance(string UrlConnection, string ProviderName)
        {
            try
            {
                if (ManagerComponents.GenCon == null)
                {
                    ManagerComponents.GenCon = new CommonConsumer(UrlConnection, ProviderName);
                    ManagerComponents.OlderUrlCommonConn = UrlConnection;
                    return ManagerComponents.GenCon;
                }
                else if (!ManagerComponents.OlderUrlCommonConn.Equals(UrlConnection))
                {
                    ManagerComponents.GenCon = new CommonConsumer(UrlConnection, ProviderName);
                    ManagerComponents.OlderUrlCommonConn = UrlConnection;
                    return ManagerComponents.GenCon;
                }
                else
                    return ManagerComponents.GenCon;
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
        /// <param name="UrlConnection">Cadena de conexion a la base de datos a consumir</param>
        /// <param name="ProviderName">Cadena que indica el proveedor del DBMS a manipular</param>
        /// <returns>Regresa una nueva instancia del tipo "DataConsumer"</returns>
        public static DataConsumer GetNewInstance(string UrlConnection, string ProviderName)
        {
            try
            {
                return new CommonConsumer(UrlConnection, ProviderName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Provee un listado con los nombre de Proveedores soportados por el Framework .NET
        /// </summary>
        /// <param name="ProviderName"></param> 
        /// <returns>System.Data.DataTable object</returns>
        public static System.Data.DataTable GetProvidersSupport()
        {
            try
            {
                return Provider.GetProviders();
            }
            catch
            {
                throw;
            }
        }
    }
}