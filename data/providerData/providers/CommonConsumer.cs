namespace data.providerData.providers;

using System.Data;
using System.Data.Common;
using data.providerData.components;

internal sealed class commonConsumer : dataConsumer
{
    #region Variables

    private DbConnection dbConnection;
    private DbCommand dbCommand;
    private DbDataAdapter dAdapter;
    private DbTransaction dbTransaction;
    private string dbProvider = string.Empty;

    #endregion

    #region Polimorphic public properties

    public override string serverVersion { get { return dbConnection.ServerVersion; } }
    public override string connectionString { get { return dbConnection.ConnectionString; } }
    public override int connectionTimeout { get { return dbConnection.ConnectionTimeout; } }
    public override string database { get { return dbConnection.Database; } }
    public override string dataSource { get { return dbConnection.DataSource; } }
    public override string workstationId { get { return null; } }
    public override string providerOrDriver { get { return this.dbProvider; } }
    public override ConnectionState dbConnectionState { get { return dbConnection.State; } }
    public override Boolean autoOpenAndCloseConnectionForDataReader { get; set; }

    #endregion

    #region Constructor

    public commonConsumer(string urlConn, string providerName)
    {
        try
        {
            this.autoOpenAndCloseConnectionForDataReader = false;
            this.dbProvider = providerName;
            this.urlConnection = urlConn;
            this.dbConnection = provider.getDbFactory(providerName).CreateConnection();
            this.dbConnection.ConnectionString = this.urlConnection;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region Polimorphic public methods

    public override void openConnection()
    {
        this.dbConnection.Open();
    }

    public override void closeConnection()
    {
        if (this.dbConnection.State == ConnectionState.Open &&
            this.dbConnection.State != ConnectionState.Executing)
        {
            this.dbConnection.Close();

            if (this.dbCommand != null)
                this.dbCommand.Dispose();
        }
        else
            throw new Exception("No se puede cerrar la conexion por que no hay una conexion activa o bien se esta realizando un proceso en este momento.");
    }

    public override void initTransaction()
    {
        this.dbCommand = provider.getDbFactory(this.dbProvider).CreateCommand();

        this.dbCommand.Connection = this.dbConnection;

        this.dbCommand.CommandTimeout = 0;

        if (this.dbCommand.Connection.State == ConnectionState.Open)
            this.dbTransaction = this.dbConnection.BeginTransaction();
        else
            throw new Exception(@"A transaction could not be defined because there's no open connection defined in DBCommand, or DBCommand is null");
    }

    public override void rollbackTransaction()
    {
        this.dbTransaction.Rollback();
        /*
        if (this.dbTransaction.Connection != null)// && this.dbCommand.Transaction != null)
        {
            this.dbTransaction.Rollback();

        }
        else
            throw new Exception("No hay una transacción definida para el proceso actual.");
            */
    }

    public override void endTransactionAndCommitDB()
    {
        this.dbTransaction.Commit();
    }

    private void createCommand()
    {
        this.createCommand(CommandType.Text);
        this.dbCommand.CommandTimeout = 0;
        this.dbCommand.CommandText = this.query;
    }

    private void createCommand(CommandType commandtype)
    {
        this.dbCommand = provider.getDbFactory(this.dbProvider).CreateCommand();
        this.dbCommand.CommandTimeout = 0;
        this.dbCommand.CommandType = commandtype;
    }

    private void createCommand(string commandText, CommandType commandType, string Parameters)
    {
        this.createCommand(commandType);
        this.dbCommand.CommandText = commandText;
        this.dbCommand.CommandTimeout = 0;
        this.AddParametersStoreProcedure(Parameters);
    }

    private void createCommand(string commandText, CommandType commandType, IDataParameter[] Parameters)
    {
        this.createCommand(commandType);
        this.dbCommand.CommandText = commandText;
        this.dbCommand.CommandTimeout = 0;
        foreach (IDataParameter param in Parameters)
            this.dbCommand.Parameters.Add(param);
    }

    private IDataAdapter createDataAdapter()
    {
        this.dAdapter = provider.getDbFactory(this.dbProvider).CreateDataAdapter();

        this.dAdapter.SelectCommand = this.dbCommand;
        this.dAdapter.SelectCommand.Connection = this.dbConnection;

        return (IDataAdapter)this.dAdapter;
    }

    private DataTable fillDataAdapter()
    {
        try
        {
            this.createDataAdapter();

            if (this.dAdapter == null)
            {
                return null;
            }

            DataTable DT = new DataTable();
            this.dAdapter.Fill(DT);
            return DT;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            if (this.dAdapter != null)
                this.dAdapter.Dispose();
        }
    }

    private void verifyOpenConnection()
    {
        if (this.autoOpenAndCloseConnectionForDataReader)
            this.openConnection();

        if (this.dbConnection.State != ConnectionState.Open)
        {
            throw new Exception("No existe una conexión abierta.");
        }
    }

    private IDataReader executeReader()
    {
        if (this.autoOpenAndCloseConnectionForDataReader)
        {
            this.dbCommand.CommandTimeout = 0;
            return (IDataReader)this.dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }
        else
            return (IDataReader)this.dbCommand.ExecuteReader();
    }

    private object executeScalarQuery()
    {
        try
        {
            this.dbCommand.CommandTimeout = 0;
            return this.dbCommand.ExecuteScalar();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.closeConnection();
        }
    }

    public override IDataAdapter GetDataAdapter()
    {
        this.createCommand();

        return this.createDataAdapter();
    }

    public override IDataAdapter GetDataAdapter(string StoreProcedure, string Parameters)
    {
        this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

        return this.createDataAdapter();
    }

    public override IDataAdapter GetDataAdapter(string StoreProcedure, IDataParameter[] Parameters)
    {
        this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

        return this.createDataAdapter();
    }

    public override IDataReader GetDataReader()
    {
        this.createCommand();

        this.verifyOpenConnection();

        this.dbCommand.Connection = this.dbConnection;

        return this.executeReader();
    }

    public override IDataReader GetDataReader(string StoreProcedure, string Parameters)
    {
        this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

        this.verifyOpenConnection();

        this.dbCommand.Connection = this.dbConnection;

        return this.executeReader();
    }

    public override IDataReader GetDataReader(string StoreProcedure, IDataParameter[] Parameters)
    {
        this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);
        this.verifyOpenConnection();
        this.dbCommand.Connection = this.dbConnection;
        this.dbCommand.CommandTimeout = 0;
        return this.executeReader();
    }

    public override DataTable ExecuteQuery()
    {
        try
        {
            this.createCommand();

            return this.fillDataAdapter();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public override DataTable ExecuteQuery(string StoreProcedure, string Parameters)
    {
        try
        {
            this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            return this.fillDataAdapter();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public override DataTable ExecuteQuery(string StoreProcedure, IDataParameter[] Parameters)
    {
        try
        {
            createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            return fillDataAdapter();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public override Object ExecuteScalar()
    {
        this.createCommand();

        this.verifyOpenConnection();

        this.dbCommand.CommandTimeout = 0;
        this.dbCommand.Connection = this.dbConnection;

        return this.executeScalarQuery();
    }

    public override Object ExecuteScalar(string StoreProcedure, string Parameters)
    {
        this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

        this.verifyOpenConnection();
        this.dbCommand.CommandTimeout = 0;

        this.dbCommand.Connection = this.dbConnection;

        return this.executeScalarQuery();
    }

    public override Object ExecuteScalar(string StoreProcedure, IDataParameter[] Parameters)
    {
        this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

        this.verifyOpenConnection();

        this.dbCommand.Connection = this.dbConnection;

        return this.executeScalarQuery();
    }

    public override Int32 ExecuteNonQuery()
    {
        try
        {
            this.createCommand();

            this.verifyOpenConnection();

            this.dbCommand.Connection = this.dbConnection;

            return this.dbCommand.ExecuteNonQuery(); ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (this.dbConnection.State == ConnectionState.Open)
                this.closeConnection();
        }
    }

    public override Int32 ExecuteNonQuery(string StoreProcedure, string Parameters)
    {
        try
        {
            this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            this.verifyOpenConnection();

            this.dbCommand.Connection = this.dbConnection;

            return this.dbCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (this.dbConnection.State == ConnectionState.Open)
                this.closeConnection();
        }
    }

    public override Int32 ExecuteNonQuery(string QueryOrSP, IDataParameter[] Parameters, bool? IsStoreProcedure = true)
    {
        try
        {
            this.createCommand(QueryOrSP, ((bool)IsStoreProcedure) ? CommandType.StoredProcedure : CommandType.Text, Parameters);

            this.verifyOpenConnection();

            this.dbCommand.Connection = this.dbConnection;

            return this.dbCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (this.dbConnection.State == ConnectionState.Open)
                this.closeConnection();
        }
    }

    public override object ExecuteScalarTransaction(string StoreProcedure, IDataParameter[] Parameters)
    {
        if (this.dbCommand != null)
        {
            this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            return this.dbCommand.ExecuteScalar(); ;
        }
        else
            throw new Exception("Transaction is no initialized, it must be defined previously.");
    }

    public override Int32 ExecuteNonQueryTransaction()
    {
        if (this.dbCommand != null)
        {
            this.createCommand();

            this.dbCommand.Connection = this.dbConnection;
            this.dbCommand.Transaction = this.dbTransaction;

            return this.dbCommand.ExecuteNonQuery();
        }
        else
            throw new Exception("Transaction is no initialized, it must be defined previously.");
    }

    public override Int32 ExecuteNonQueryTransaction(string StoreProcedure, string Parameters)
    {
        if (this.dbCommand != null)
        {
            this.createCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            this.dbCommand.Connection = this.dbConnection;
            this.dbCommand.Transaction = this.dbTransaction;

            return this.dbCommand.ExecuteNonQuery();
        }
        else
            throw new Exception("Transaction is no initialized, it must be defined previously.");
    }

    public override Int32 ExecuteNonQueryTransaction(string QueryOrSP, IDataParameter[] Parameters, bool? IsStoreProcedure = true)
    {
        if (this.dbCommand != null)
        {
            this.createCommand(QueryOrSP, ((bool)IsStoreProcedure) ? CommandType.StoredProcedure : CommandType.Text, Parameters);

            this.dbCommand.Connection = this.dbConnection;
            this.dbCommand.Transaction = this.dbTransaction;

            return this.dbCommand.ExecuteNonQuery();
        }
        else
            throw new Exception("Transaction is no initialized, it must be defined previously.");
    }

    public override void Dispose()
    {
        if (this.dbTransaction != null)
            this.dbTransaction.Dispose();

        if (this.dAdapter != null)
            this.dAdapter.Dispose();

        if (this.dbCommand != null)
            this.dbCommand.Dispose();

        if (this.dbConnection != null)
            this.dbConnection.Dispose();

        GC.SuppressFinalize(this);

        managerComponents.disposeComponent();
    }

    #endregion

    #region Metodos Privados

    private void AddParametersStoreProcedure(string Parameters)
    {
        if (Parameters.Length.Equals(0)) return;

        if (this.dbCommand != null)
        {
            this.dbCommand.Parameters.Clear();

            string[] detParam = Parameters.Split(',');

            foreach (string param in detParam)
            {
                if (param.Equals(string.Empty)) continue;

                string[] Item = param.Split('=');

                if (Item.Length.Equals(2))
                {
                    DbParameter parameter = provider.getDbFactory(this.dbProvider).CreateParameter();
                    parameter.ParameterName = Item[0];
                    parameter.Value = Item[1];
                    this.dbCommand.Parameters.Add(parameter);
                }
            }
        }
        else
            throw new Exception("DBCommand is null, it's not possible to assign Stored Procedure parameters");
    }

    #endregion
}