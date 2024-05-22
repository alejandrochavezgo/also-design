using System.Data;
using System.Data.Common;
using data.providerData.components;

namespace data.providerData.providers
{
    internal sealed class CommonConsumer : DataConsumer
    {
        #region Variables

        private DbConnection dbConnection;
        private DbCommand dbCommand;
        private DbDataAdapter DAdapter;
        private DbTransaction dbTransaction;
        private string dbProvider = string.Empty;

        #endregion

        #region Polimorphic public properties

        public override string ServerVersion { get { return dbConnection.ServerVersion; } }
        public override string ConnectionString { get { return dbConnection.ConnectionString; } }
        public override int ConnectionTimeout { get { return dbConnection.ConnectionTimeout; } }
        public override string Database { get { return dbConnection.Database; } }
        public override string DataSource { get { return dbConnection.DataSource; } }
        public override string WorkstationId { get { return null; } }
        public override string ProviderOrDriver { get { return this.dbProvider; } }
        public override ConnectionState DbConnectionState { get { return dbConnection.State; } }
        public override Boolean AutoOpenAndCloseConnectionForDataReader { get; set; }

        #endregion

        #region Constructor

        public CommonConsumer(string UrlConn, string ProviderName)
        {
            try
            {
                this.AutoOpenAndCloseConnectionForDataReader = false;
                this.dbProvider = ProviderName;
                this.UrlConnection = UrlConn;
                this.dbConnection = Provider.GetDbFactory(ProviderName).CreateConnection();
                this.dbConnection.ConnectionString = this.UrlConnection;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Polimorphic public methods

        public override void OpenConnection()
        {
            this.dbConnection.Open();
        }

        public override void CloseConnection()
        {
            if (this.dbConnection.State == ConnectionState.Open &&
                this.dbConnection.State != ConnectionState.Executing)
            {
                this.dbConnection.Close();

                if (this.dbCommand != null)
                    this.dbCommand.Dispose();
            }
            else
                throw new Exception("No se puede cerrar la conexion por que no hay una conexion activa " +
                                    "o bien se esta realizando un proceso en este momento.");
        }

        public override void InitTransaction()
        {
            this.dbCommand = Provider.GetDbFactory(this.dbProvider).CreateCommand();

            this.dbCommand.Connection = this.dbConnection;

            this.dbCommand.CommandTimeout = 0;

            if (this.dbCommand.Connection.State == ConnectionState.Open)
                this.dbTransaction = this.dbConnection.BeginTransaction();
            else
                throw new Exception(@"A transaction could not be defined because there's no open connection defined in DBCommand,
                                      or DBCommand is null");
        }

        public override void RollbackTransaction()
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

        public override void EndTransactionAndCommitDB()
        {
            this.dbTransaction.Commit();
        }

        private void CreateCommand()
        {
            this.CreateCommand(CommandType.Text);
            this.dbCommand.CommandTimeout = 0;
            this.dbCommand.CommandText = this.Query;
        }

        private void CreateCommand(CommandType commandtype)
        {
            this.dbCommand = Provider.GetDbFactory(this.dbProvider).CreateCommand();
            this.dbCommand.CommandTimeout = 0;
            this.dbCommand.CommandType = commandtype;
        }

        private void CreateCommand(string commandText, CommandType commandType, string Parameters)
        {
            this.CreateCommand(commandType);
            this.dbCommand.CommandText = commandText;
            this.dbCommand.CommandTimeout = 0;
            this.AddParametersStoreProcedure(Parameters);
        }

        private void CreateCommand(string commandText, CommandType commandType, IDataParameter[] Parameters)
        {
            this.CreateCommand(commandType);
            this.dbCommand.CommandText = commandText;
            this.dbCommand.CommandTimeout = 0;
            foreach (IDataParameter param in Parameters)
                this.dbCommand.Parameters.Add(param);
        }

        private IDataAdapter CreateDataAdapter()
        {
            this.DAdapter = Provider.GetDbFactory(this.dbProvider).CreateDataAdapter();

            this.DAdapter.SelectCommand = this.dbCommand;
            this.DAdapter.SelectCommand.Connection = this.dbConnection;

            return (IDataAdapter)this.DAdapter;
        }

        private DataTable FillDataAdapter()
        {
            try
            {
                this.CreateDataAdapter();

                if (this.DAdapter == null)
                {
                    return null;
                }

                DataTable DT = new DataTable();
                this.DAdapter.Fill(DT);
                return DT;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this.DAdapter != null)
                    this.DAdapter.Dispose();
            }
        }

        private void VerifyOpenConnection()
        {
            if (this.AutoOpenAndCloseConnectionForDataReader)
                this.OpenConnection();

            if (this.dbConnection.State != ConnectionState.Open)
            {
                throw new Exception("No existe una conexión abierta.");
            }
        }

        private IDataReader ExecuteReader()
        {
            if (this.AutoOpenAndCloseConnectionForDataReader)
            {
                this.dbCommand.CommandTimeout = 0;
                return (IDataReader)this.dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            else
                return (IDataReader)this.dbCommand.ExecuteReader();
        }

        private object ExecuteScalarQuery()
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
                this.CloseConnection();
            }
        }

        public override IDataAdapter GetDataAdapter()
        {
            this.CreateCommand();

            return this.CreateDataAdapter();
        }

        public override IDataAdapter GetDataAdapter(string StoreProcedure, string Parameters)
        {
            this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            return this.CreateDataAdapter();
        }

        public override IDataAdapter GetDataAdapter(string StoreProcedure, IDataParameter[] Parameters)
        {
            this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            return this.CreateDataAdapter();
        }

        public override IDataReader GetDataReader()
        {
            this.CreateCommand();

            this.VerifyOpenConnection();

            this.dbCommand.Connection = this.dbConnection;

            return this.ExecuteReader();
        }

        public override IDataReader GetDataReader(string StoreProcedure, string Parameters)
        {
            this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            this.VerifyOpenConnection();

            this.dbCommand.Connection = this.dbConnection;

            return this.ExecuteReader();
        }

        public override IDataReader GetDataReader(string StoreProcedure, IDataParameter[] Parameters)
        {
            this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);
            this.VerifyOpenConnection();
            this.dbCommand.Connection = this.dbConnection;
            this.dbCommand.CommandTimeout = 0;
            return this.ExecuteReader();
        }

        public override DataTable ExecuteQuery()
        {
            try
            {
                this.CreateCommand();

                return this.FillDataAdapter();
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
                this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

                return this.FillDataAdapter();
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
                CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

                return FillDataAdapter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override Object ExecuteScalar()
        {
            this.CreateCommand();

            this.VerifyOpenConnection();

            this.dbCommand.CommandTimeout = 0;
            this.dbCommand.Connection = this.dbConnection;

            return this.ExecuteScalarQuery();
        }

        public override Object ExecuteScalar(string StoreProcedure, string Parameters)
        {
            this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            this.VerifyOpenConnection();
            this.dbCommand.CommandTimeout = 0;

            this.dbCommand.Connection = this.dbConnection;

            return this.ExecuteScalarQuery();
        }

        public override Object ExecuteScalar(string StoreProcedure, IDataParameter[] Parameters)
        {
            this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

            this.VerifyOpenConnection();

            this.dbCommand.Connection = this.dbConnection;

            return this.ExecuteScalarQuery();
        }

        public override Int32 ExecuteNonQuery()
        {
            try
            {
                this.CreateCommand();

                this.VerifyOpenConnection();

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
                    this.CloseConnection();
            }
        }

        public override Int32 ExecuteNonQuery(string StoreProcedure, string Parameters)
        {
            try
            {
                this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

                this.VerifyOpenConnection();

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
                    this.CloseConnection();
            }
        }

        public override Int32 ExecuteNonQuery(string QueryOrSP, IDataParameter[] Parameters, bool? IsStoreProcedure = true)
        {
            try
            {
                this.CreateCommand(QueryOrSP, ((bool)IsStoreProcedure) ? CommandType.StoredProcedure : CommandType.Text, Parameters);

                this.VerifyOpenConnection();

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
                    this.CloseConnection();
            }
        }

        public override object ExecuteScalarTransaction(string StoreProcedure, IDataParameter[] Parameters)
        {
            if (this.dbCommand != null)
            {
                this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

                return this.dbCommand.ExecuteScalar(); ;
            }
            else
                throw new Exception("Transaction is no initialized, it must be defined previously.");
        }

        public override Int32 ExecuteNonQueryTransaction()
        {
            if (this.dbCommand != null)
            {
                this.CreateCommand();

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
                this.CreateCommand(StoreProcedure, CommandType.StoredProcedure, Parameters);

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
                this.CreateCommand(QueryOrSP, ((bool)IsStoreProcedure) ? CommandType.StoredProcedure : CommandType.Text, Parameters);

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

            if (this.DAdapter != null)
                this.DAdapter.Dispose();

            if (this.dbCommand != null)
                this.dbCommand.Dispose();

            if (this.dbConnection != null)
                this.dbConnection.Dispose();

            GC.SuppressFinalize(this);

            ManagerComponents.DisposeComponent();
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
                        DbParameter parameter = Provider.GetDbFactory(this.dbProvider).CreateParameter();
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
}
