using System.Data;

namespace data.providerData
{
    /// <summary>
    /// Clase generica para la manipulacion de Base de Datos. Actualmente soporta varios manejadores(DBMS).
    /// Vea las propiedades del Enumerador DBMS para ver los manejadores soportados.
    /// Esta clase no se puede instanciar con "new". Para generar una intancia se debe llamar al metodo
    /// "DataConsumer_InstanceNew" de la clase "DataFactory".
    /// Este componente esta basado en el Patron de Diseño "Simple Factory".
    /// </summary>
    /// <Author>Juan C. Escobar Hernández</Author>

    public abstract class DataConsumer : IDisposable
    {
        #region Private variables
        private string query = string.Empty;
        private string urlconnection = string.Empty;
        #endregion

        #region Heritable properties
        protected string UrlConnection
        {
            get
            {
                if (urlconnection.Equals(string.Empty))
                    throw new Exception("Debe indicar la Url de conexion a la BD.");
                else
                    return urlconnection.Replace("\t", " ").Replace("\n", " ").Replace("\r", " ");
            }
            set
            {
                if (value.Equals(string.Empty))
                    throw new Exception("No se indico la Url de conexion a la BD.");
                else
                    urlconnection = value;
            }
        }
        #endregion

        #region Public properties
        /// <summary>
        ///  Propiedad Get/Set. Indica la sentencia SQL que se va a ejecutar.
        /// </summary> 
        public string Query
        {
            get
            {
                if (query.Equals(string.Empty))
                    throw new Exception("Debe indicar una sentencia SQL a ejecutar.");
                else
                    return query.Replace("\t", " ").Replace("\n", " ").Replace("\r", " ");
            }
            set
            {
                if (value.Equals(string.Empty))
                    throw new Exception("No se indico la sentencia SQL a ejecutar.");
                else
                    query = value;
            }
        }
        /// <summary>
        /// Propiedad Get. Indica la version del servidor de BD que se esta consumiendo.
        /// </summary>
        public abstract string ServerVersion { get; }
        /// <summary>
        /// Propiedad Get. URL de la de conexion a la BD que se definio.
        /// </summary>
        public abstract string ConnectionString { get; }
        /// <summary>
        /// Propiedad Get. Indica el tiempo maximo en que se mantendra la conexion a la BD.
        /// </summary>
        public abstract int ConnectionTimeout { get; }
        /// <summary>
        /// Propiedad Get. Nombre de la instancia del catalogo de la BD a consumir.
        /// </summary>
        public abstract string Database { get; }
        /// <summary>
        /// Propiedad Get. Servidor fisico (IP/DNS) donde se encuentra alojada la Instancia de BD a consumir.
        /// </summary>
        public abstract string DataSource { get; }
        /// <summary>
        /// Propiedad Get. Si la terminal remota es una Estacion de Trabajo se devuelve su ID, si no se regresa un nulo.
        /// </summary>
        public abstract string WorkstationId { get; }
        /// <summary>
        /// Propiedad Get. Da informacion del Provedor o Driver indicado para conexiones OLE o ODBC.
        /// </summary>
        public abstract string ProviderOrDriver { get; }
        /// <summary>
        /// Propiedad Get. Indica el estado de la conexion.
        /// </summary>
        public abstract ConnectionState DbConnectionState { get; }

        /// <summary>
        /// Propiedad Get/Set que indica si se manejara en automatico 
        /// la apertura y cierre de la conexion al manejar un IDataReader
        /// </summary>
        public abstract Boolean AutoOpenAndCloseConnectionForDataReader { get; set; }
        #endregion

        #region Public methods
        /// <summary>
        /// Habre una conexion a la instancia de BD indicada
        /// </summary>
        public abstract void OpenConnection();
        /// <summary>
        /// Cierra la conexion a la instancia de BD indicada
        /// </summary>
        public abstract void CloseConnection();

        /// <summary>
        /// Proporciona una interface del tipo System.Data.IDataAdapter para tratamiento personalizado
        /// de registros en la base de datos, ya sea con objetos del tipo System.Data.DataSet, System.Data.DataTable, etc.
        /// Previamente se requiere haber definido la consulta SQL en la propiedad "Query".
        /// No es necesario previamente abrir ni cerrar la conexion a la BD.
        /// </summary>
        public abstract IDataAdapter GetDataAdapter();
        /// <summary>
        /// Proporciona una interface del tipo System.Data.IDataAdapter para tratamiento personalizado
        /// de registros devueltos por un Store Procedure en la base de datos, ya sea con objetos del tipo 
        /// System.Data.DataSet, System.Data.DataTable, etc.
        /// No es necesario previamente abrir ni cerrar la conexion a la BD.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del Store Procedure a ejecutar</param>
        /// <param name="Parameters">Parametros que recibe el Store Procedure. 
        /// La forma generica es "Parametro-1=Valor-1,Parametro-2=Valor-2,...,Parametro-N=Valor-N,"</param>
        /// <returns>Regresa un adaptador de datos del tipo IDataAdapter</returns>
        public abstract IDataAdapter GetDataAdapter(string StoreProcedure, string Parameters);
        /// <summary>
        /// Proporciona una interface del tipo System.Data.IDataAdapter para tratamiento personalizado
        /// de registros devueltos por un Store Procedure en la base de datos, ya sea con objetos del tipo 
        /// System.Data.DataSet, System.Data.DataTable, etc.
        /// No es necesario previamente abrir ni cerrar la conexion a la BD.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del StoreProcedure que se ejecutara</param>
        /// <param name="Parameters">Un array del tipo System.Data.IDataParameter con los parametros del SP</param>
        /// <returns>Regresa un adaptador de datos del tipo System.Data.IDataAdapter</returns>
        public abstract IDataAdapter GetDataAdapter(string StoreProcedure, IDataParameter[] Parameters);

        /// <summary>
        /// Ejecuta la consulta SQL indicada regresando una interface del tipo System.Data.IDataReader para recorrer los registros devueltos.
        /// Se debe previamente abrir la conexion a la BD y mantenerla abierta hasta que se termine de recorrer
        /// el IDataReader, posteriormente se debe cerrar el System.Data.IDataParameter y cerrar la conexion.
        /// Se requiere que previamente se haya definido la sentencia SQL a ejecutar en la propiedad "Query".
        /// </summary>
        /// <returns>Un objeto para lectura del tipo: System.Data.IDataReader</returns>
        public abstract IDataReader GetDataReader();
        /// <summary>
        /// Ejecuta la consulta SQL de un Store Procedure indicado regresando una interface del tipo System.Data.IDataReader 
        /// para recorrer los registros devueltos.
        /// Se debe previamente abrir la conexion a la BD y mantenerla abierta hasta que se termine de recorrer
        /// el IDataReader, posteriormente se debe cerrar el System.Data.IDataParameter y cerrar la conexion.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del Store Procedure a ejecutar</param>
        /// <param name="Parameters">Parametros que recibe el Store Procedure. 
        /// La forma generica es "Parametro-1=Valor-1,Parametro-2=Valor-2,...,Parametro-N=Valor-N,"</param>
        /// <returns>Un objeto para lectura del tipo: System.Data.IDataReader</returns>
        public abstract IDataReader GetDataReader(string StoreProcedure, string Parameters);
        /// <summary>
        /// Ejecuta la consulta SQL indicada regresando una interface del tipo System.Data.IDataReader para recorrer los registros devueltos.
        /// Se debe previamente abrir la conexion a la BD y mantenerla abierta hasta que se termine de recorrer
        /// el IDataReader, posteriormente se debe cerrar el System.Data.IDataParameter y cerrar la conexion.
        /// Se requiere que previamente se haya definido la sentencia SQL a ejecutar en la propiedad "Query".
        ///</summary>
        /// <param name="StoreProcedure">Nombre del StoreProcedure a ejecutar</param>
        /// <param name="Parameters">Un array del tipo System.Data.IDataParameter con los parametros del SP</param>
        /// <returns>Un objeto para lectura del tipo: System.Data.IDataReader</returns>
        public abstract IDataReader GetDataReader(string StoreProcedure, IDataParameter[] Parameters);

        /// <summary>
        /// Ejecuta la consulta SQL indicada regresando una tabla con el resultado del Query.
        /// No es necesario habrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// Se requiere que previamente se haya definido la sentencia SQL a ejecutar en la propiedad "Query".
        /// </summary>
        /// <returns>Un objeto del tipo: System.Data.DataTable</returns>
        public abstract DataTable ExecuteQuery();
        /// <summary>
        /// Ejecuta un Store Procedure indicado regresando una tabla con los registros devueltos.
        /// No es necesario habrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del Store Procedure a ejecutar</param>
        /// <param name="Parameters">Parametros que recibe el Store Procedure. 
        /// La forma generica es "Parametro-1=Valor-1,Parametro-2=Valor-2,...,Parametro-N=Valor-N,"</param>
        /// <returns>Un objeto del tipo: System.Data.DataTable</returns>
        public abstract DataTable ExecuteQuery(string StoreProcedure, string Parameters);
        /// <summary>
        /// Ejecuta un Store Procedure indicado regresando una tabla con los registros devueltos.
        /// No es necesario habrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del Store Procedure a ejecutar</param>
        /// <param name="Parameters">Un array del tipo System.Data.IDataParameter con los parametros del SP</param>
        /// <returns>Un objeto del tipo: System.Data.DataTable</returns>
        public abstract DataTable ExecuteQuery(string StoreProcedure, IDataParameter[] Parameters);

        /// <summary>
        /// Ejecuta la consulta SQL indicada regresando un unico valor el cual puede ser parseado a un tipo
        /// Primitivo(int,double, etc) o Cumpuesto Simple(String, etc). 
        /// No es necesario abrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// Se requiere que previamente se haya definido la sentencia SQL a ejecutar en la propiedad "Query".
        /// </summary>
        /// <returns>Un objeto generico para parsear al tipo requerido segun la respuesta de la sentencia SQL</returns>
        public abstract Object ExecuteScalar();
        /// <summary>
        /// Ejecuta un Store Procedure regresando un unico valor el cual puede ser parseado a un tipo
        /// Primitivo(int,double, etc) o Cumpuesto Simple(String, etc). 
        /// No es necesario abrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del Store Procedure a ejecutar</param>
        /// <param name="Parameters">Parametros que recibe el Store Procedure. 
        /// La forma generica es "Parametro-1=Valor-1,Parametro-2=Valor-2,...,Parametro-N=Valor-N,"</param>
        /// <returns>Un objeto generico para parsear al tipo requerido segun la respuesta de la sentencia SQL</returns>
        public abstract Object ExecuteScalar(string StoreProcedure, string Parameters);
        /// <summary>
        /// /// Ejecuta un Store Procedure regresando un unico valor el cual puede ser parseado a un tipo
        /// Primitivo(int,double, etc) o Cumpuesto Simple(String, etc). 
        /// No es necesario abrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del Store Procedure a ejecutar</param>
        /// <param name="Parameters">Un array del tipo System.Data.IDataParameter con los parametros del SP</param>
        /// <returns>Un objeto generico para parsear al tipo requerido segun la respuesta de la sentencia SQL</returns>
        public abstract Object ExecuteScalar(string StoreProcedure, IDataParameter[] Parameters);
        /// <summary>
        /// Ejecuta un Store Procedure dentro de una transacción regresando un unico valor el cual puede ser parseado a un tipo
        /// Primitivo(int,double, etc) o Cumpuesto Simple(String, etc). Se requiere tener la transacción iniciada.
        /// No es necesario abrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del Store Procedure a ejecutar</param>
        /// <param name="Parameters">Un array del tipo System.Data.IDataParameter con los parametros del SP</param>
        /// <returns>Un objeto generico para parsear al tipo requerido segun la respuesta de la sentencia SQL</returns>
        public abstract Object ExecuteScalarTransaction(string StoreProcedure, IDataParameter[] Parameters);

        /// <summary>
        /// Ejecuta la sentencia SQL indicada que afectara los registros en la BD. 
        /// No es necesario abrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// Se requiere que previamente se haya definido la sentencia SQL a ejecutar en la propiedad "Query".
        /// </summary>
        /// <returns>Regresa el numero de registros afectados en la BD</returns>
        public abstract Int32 ExecuteNonQuery();
        /// <summary>
        /// Ejecuta la sentencia SQL indicada de un Store Procedure que no devuelve datos. 
        /// No es necesario abrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del StoreProcedure que se ejecutara</param>
        /// <param name="Parameters">Parametros que recibe el Store Procedure. 
        /// La forma generica es "Parametro-1=Valor-1,Parametro-2=Valor-2,...,Parametro-N=Valor-N,"</param>
        /// <returns>Regresa el estado de término del Store Procedure</returns>
        public abstract Int32 ExecuteNonQuery(string StoreProcedure, string Parameters);
        /// <summary>
        ///  Ejecuta la sentencia SQL indicada de un Store Procedure que no devuelve datos. 
        /// No es necesario abrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del StoreProcedure que se ejecutara</param>
        /// <param name="Parameters">Un array del tipo System.Data.IDataParameter con los parametros del SP</param>
        /// <returns>Regresa el estado de término del Store Procedure</returns>
        public abstract Int32 ExecuteNonQuery(string QueryOrSP, IDataParameter[] Parameters, bool? IsStoreProcedure = true);
        /// <summary>
        /// Inicia una nueva transaccion en la BD, se requiere previamente abrir la conexion a la BD
        /// </summary>

        public abstract void InitTransaction();
        /// <summary>
        /// Finaliza una transaccion previamente iniciada y guarda los cambios hechos en la BD
        /// </summary>
        public abstract void EndTransactionAndCommitDB();
        /// <summary>
        /// Hace un reverso de los registros afectados por una transaccion fallida en la BD.
        /// </summary>
        public abstract void RollbackTransaction();
        /// <summary>
        /// Ejecuta una sentencia SQL dentro de una transaccion previamente iniciada. 
        /// Previamente se debe abrir una conexion con "OpenConnection()", posteriormente se debe iniciar una nueva 
        /// transaccion con "InitTransaction()", despues se define la sentencia SQL en la propiedad "Query" para posteriormente
        /// ejecutar este metodo, por cada paso dentro de la transaccion se debe hacer una redefinicion de la siguiente 
        /// sentencia SQL a ejecutar en la propiedad "Query" y volver a ejectuar este metodo. Una ves terminado el proceso en los
        /// registros de la BD y si todo el proceso es satisfactorio se llama al metodo "EndTransactionAndCommitDB()" para
        /// guardar los cambios, en caso contrario se llama a "RollbackTransaction()" para cancelar los registros afectados 
        /// en la BD. Finalmente se llama a "CloseConnection()" para cerrar la conexion y finalizar el proceso de transaccion.
        /// </summary>
        /// <returns>Regresa el numero de registros afectados en la BD</returns>
        public abstract Int32 ExecuteNonQueryTransaction();
        /// <summary>
        /// Ejecuta la sentencia SQL indicada de un Store Procedure que no devuelve datos bajo una transacción previamente iniciada. 
        /// </summary>
        /// <param name="StoreProcedure">Nombre del StoreProcedure que se ejecutara</param>
        /// <param name="Parameters">Parametros que recibe el Store Procedure. 
        /// La forma generica es "Parametro-1=Valor-1,Parametro-2=Valor-2,...,Parametro-N=Valor-N,"</param>
        /// <returns>Regresa el estado de término del Store Procedure</returns>
        public abstract Int32 ExecuteNonQueryTransaction(string StoreProcedure, string Parameters);
        /// <summary>
        ///  Ejecuta la sentencia SQL indicada de un Store Procedure que no devuelve datos bajo una transacción previamente iniciada.
        /// No es necesario abrir previamente una conexion ni cerrar la conexion despues de la llamada a este metodo.
        /// </summary>
        /// <param name="StoreProcedure">Nombre del StoreProcedure que se ejecutara</param>
        /// <param name="Parameters">Un array del tipo System.Data.IDataParameter con los parametros del SP</param>
        /// <returns>Regresa el estado de término del Store Procedure</returns>
        public abstract Int32 ExecuteNonQueryTransaction(string QueryOrSP, IDataParameter[] Parameters, bool? IsStoreProcedure = true);

        /// <summary>
        /// Metodo que libera los recursos del objeto
        /// </summary>
        public abstract void Dispose();
        #endregion
    }
}