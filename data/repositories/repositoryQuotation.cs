namespace data.repositories;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using common.configurations;
using common.logging;
using data.factoryInstances;
using data.providerData;
using entities.models;
using Newtonsoft.Json;

public class repositoryQuotation : baseRepository
{
    private log _logger;

    public repositoryQuotation()
    {
        _logger = new log();
    }

    public List<quotationModel> getQuotations()
    {
        try
        {
            return factoryGetQuotations.getList((DbDataReader)_providerDB.GetDataReader("sp_getQuotations", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public quotationModel getQuotationById(int id)
    {
        try
        {
            return factoryGetQuotationById.get((DbDataReader)_providerDB.GetDataReader("sp_getQuotationById", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@quotationId", DbType.Int32, id),
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getPaymentTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getPaymentTypesCatalog", new DbParameter[]{}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getPackingUnitTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getPackingUnitTypesCatalog", new DbParameter[]{}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getCurrencyTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getCurrencyTypesCatalog", new DbParameter[]{}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<quotationItemsModel> getQuotationItemsByIdQuotation(int id)
    {
        try
        {
            return factoryGetQuotationItemsByIdQuotation.getList((DbDataReader)_providerDB.GetDataReader("sp_getQuotationItemsByIdQuotation", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@quotationId", DbType.Int32, id),
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public int addQuotation(quotationModel quotation)
    {
        try
        {
            var quotationIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@quotationIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addQuotation", new DbParameter[] {
                quotationIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, quotation.client!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId", DbType.Int32, quotation.user!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, quotation.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@paymentId", DbType.Int32, quotation.payment!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@currencyId", DbType.Int32, quotation.currency!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@subtotal", DbType.Decimal, quotation.subtotal!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@taxRate", DbType.Decimal, quotation.taxRate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@taxAmount", DbType.Decimal, quotation.taxAmount!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@totalAmount", DbType.Decimal, quotation.totalAmount!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@creationDate", DbType.DateTime, quotation.creationDate),
                dataFactory.getObjParameter(configurationManager.providerDB,"@userMainContactPhone", DbType.String, quotation.user.employee!.mainContactPhone!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientMainContactName", DbType.String, quotation.client!.mainContactName!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientMainContactPhone", DbType.String, quotation.client!.mainContactPhone!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@generalNotes", DbType.String, quotation.generalNotes!)
            });
            return Convert.ToInt32(quotationIdAdded.Value);
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public int updateQuotation(quotationModel quotation)
    {
        try
        {
            var quotationIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@quotationIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_updateQuotation", new DbParameter[] {
                quotationIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@quotationId", DbType.Int32, quotation!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, quotation.client!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId", DbType.Int32, quotation.user!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, quotation.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@paymentId", DbType.Int32, quotation.payment!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@currencyId", DbType.Int32, quotation.currency!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@subtotal", DbType.Decimal, quotation.subtotal!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@taxRate", DbType.Decimal, quotation.taxRate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@taxAmount", DbType.Decimal, quotation.taxAmount!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@totalAmount", DbType.Decimal, quotation.totalAmount!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@modificationDate", DbType.DateTime, quotation.modificationDate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@userMainContactPhone", DbType.String, quotation.user.employee!.mainContactPhone!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientMainContactName", DbType.String, quotation.client!.mainContactName!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientMainContactPhone", DbType.String, quotation.client!.mainContactPhone!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@generalNotes", DbType.String, quotation.generalNotes!)
            });

            return Convert.ToInt32(quotationIdUpdated.Value);
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public int addQuotationItem(quotationItemsModel quotationItem, int quotationId)
    {
        try
        {
            var quotationItemIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@quotationItemIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addQuotationItem", new DbParameter[] {
                quotationItemIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@quotationId", DbType.Int32, quotationId),
                dataFactory.getObjParameter(configurationManager.providerDB,"@quantity", DbType.Int32, quotationItem.quantity),
                dataFactory.getObjParameter(configurationManager.providerDB,"@unit", DbType.Int32, quotationItem.unit!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@unitValue", DbType.Decimal, quotationItem.unitValue),
                dataFactory.getObjParameter(configurationManager.providerDB,"@totalValue", DbType.Decimal, quotationItem.totalValue),
                dataFactory.getObjParameter(configurationManager.providerDB,"@description", DbType.String, quotationItem.description!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@material", DbType.String, quotationItem.material!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@details", DbType.String, quotationItem.details!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@imagePath", DbType.String, quotationItem.imagePath!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@notes", DbType.String, quotationItem.notes!)
            });

            return Convert.ToInt32(quotationItemIdAdded.Value);
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public int updateQuotationItem(quotationItemsModel quotationItem)
    {
        try
        {
            var quotationItemIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@quotationItemIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_updateQuotationItem", new DbParameter[] {
                quotationItemIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@quotationItemId", DbType.Int32, quotationItem.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@quantity", DbType.Int32, quotationItem.quantity),
                dataFactory.getObjParameter(configurationManager.providerDB,"@unit", DbType.Int32, quotationItem.unit!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@unitValue", DbType.Decimal, quotationItem.unitValue),
                dataFactory.getObjParameter(configurationManager.providerDB,"@totalValue", DbType.Decimal, quotationItem.totalValue),
                dataFactory.getObjParameter(configurationManager.providerDB,"@description", DbType.String, quotationItem.description!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@material", DbType.String, quotationItem.material!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@details", DbType.String, quotationItem.details!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@imagePath", DbType.String, quotationItem.imagePath!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@notes", DbType.String, quotationItem.notes!)
            });
            return Convert.ToInt32(quotationItemIdUpdated.Value);
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool deleteQuotationById(int id)
    {
        try
        {
            base._providerDB.ExecuteNonQuery("sp_deleteQuotationById", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@quotationId", DbType.Int32, id),
            });
            return true;
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool updateStatusByQuotationById(int quotationId, int status)
    {
        try
        {
            var idQuotationUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@idQuotationUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_updateStatusByQuotationId", new DbParameter[] {
                idQuotationUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@quotationId", DbType.Int32, quotationId),
                dataFactory.getObjParameter(configurationManager.providerDB,"@newStatus", DbType.Int32, status)
            });

            return Convert.ToInt32(idQuotationUpdated.Value) > 0 ? true : false;
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<traceModel> getQuotationTracesByQuotationId(int id)
    {
        try
        {
            return factoryGetQuotationTracesByQuotationId.getList((DbDataReader)_providerDB.GetDataReader("sp_getLastQuotationTracesByQuotationId", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB, "@quotationId", DbType.Int32, id)
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public traceModel getQuotationTraceById(int id)
    {
        try
        {
            return factoryGetQuotationTraceById.get((DbDataReader)_providerDB.GetDataReader("sp_getQuotationTraceById", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB, "@traceId", DbType.Int32, id)
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<quotationModel> getQuotationsByTerm(string code)
    {
        try
        {
            return factoryGetQuotationsByTerm.getList((DbDataReader)_providerDB.GetDataReader("sp_getQuotationsByTerm", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@code", DbType.String, code)
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}