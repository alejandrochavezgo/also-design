namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using entities.enums;
using Newtonsoft.Json;
using System.Transactions;
using common.utils;
using common.helpers;

public class facadeQuotation
{
    private log _logger;
    private userModel _user;
    private facadeTrace _facadeTrace;
    private repositoryQuotation _repositoryQuotation;
    private DateTime _dateTime;

    public facadeQuotation(userModel user)
    {
        _user = user;
        _logger = new log();
        _facadeTrace = new facadeTrace();
        _repositoryQuotation = new repositoryQuotation();
        _dateTime = new dateHelper().pstNow();
    }

    public List<quotationModel> getQuotations()
    {
        try
        {
            return _repositoryQuotation.getQuotations();
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool addQuotation(quotationModel quotation)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                quotation.creationDate = _dateTime;
                var quotationIdAdded = _repositoryQuotation.addQuotation(quotation);

                foreach (var item in quotation.items)
                {
                    item.imagePath = string.IsNullOrEmpty(item.imagePath) ? string.Empty : item.imagePath;
                    if(!(_repositoryQuotation.addQuotationItem(item, quotationIdAdded) > 0))
                        throw new Exception($"Error at saving the quotation item.");
                }

                var result = quotationIdAdded > 0;
                var quotationAfter = getQuotationById(quotationIdAdded);
                var quotationSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_QUOTATION,
                    entityType = entityType.QUOTATION,
                    userId = _user.id,
                    comments = "QUOTATION ADDED.",
                    beforeChange = string.Empty,
                    afterChange = JsonConvert.SerializeObject(quotationAfter, quotationSettings),
                    entityId = quotationIdAdded
                });

                if(result && trace > 0)
                    transactionScope.Complete();
                else
                    transactionScope.Dispose();

                return result;
            }
            catch (Exception exception)
            {
                transactionScope.Dispose();
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                throw exception;
            }
        }
    }

    public List<List<catalogModel>> getAllQuotationCatalogs()
    {
        try
        {
            var catalogs = new List<List<catalogModel>>();
            catalogs.Add(_repositoryQuotation.getPaymentTypesCatalog());
            catalogs.Add(_repositoryQuotation.getCurrencyTypesCatalog());
            return catalogs;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool updateQuotation(quotationModel quotation)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                var quotationBefore = getQuotationById(quotation.id);
                if (quotationBefore != null && quotationBefore.status != (int)statusType.ACTIVE)
                {
                    transactionScope.Dispose();
                    return false;
                }

                quotation.modificationDate = _dateTime;
                var quotationIdUpdated = _repositoryQuotation.updateQuotation(quotation);

                foreach (var item in quotation.items)
                    if (item.id > 0)
                    {
                        item.imagePath = string.IsNullOrEmpty(item.imagePath) ? string.Empty : item.imagePath;
                        if(!(_repositoryQuotation.updateQuotationItem(item) > 0))
                            throw new Exception($"Error at updating the quotation item.");
                    }
                    else
                    {
                        item.imagePath = string.IsNullOrEmpty(item.imagePath) ? string.Empty : item.imagePath;
                        if(!(_repositoryQuotation.addQuotationItem(item, quotationIdUpdated) > 0))
                            throw new Exception($"Error at saving the new quotation item.");
                    }

                var result = quotationIdUpdated > 0;
                var quotationAfter = getQuotationById(quotation.id);
                var quotationSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.UPDATE_QUOTATION,
                    entityType = entityType.QUOTATION,
                    userId = _user.id,
                    comments = "QUOTATION UPDATED.",
                    beforeChange = JsonConvert.SerializeObject(quotationBefore, quotationSettings),
                    afterChange = JsonConvert.SerializeObject(quotationAfter, quotationSettings),
                    entityId = quotation.id
                });

                if(result && trace > 0)
                    transactionScope.Complete();
                else
                    transactionScope.Dispose();

                return result;
            }
            catch (Exception exception)
            {
                transactionScope.Dispose();
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                throw exception;
            }
        }
    }

    public bool deleteQuotationById(int id)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                var quotation = _repositoryQuotation.getQuotationById(id);
                if (quotation == null || quotation.status != (int)statusType.ACTIVE)
                    return false;

                var result = _repositoryQuotation.deleteQuotationById(id);
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.DELETE_QUOTATION,
                    entityType = entityType.QUOTATION,
                    userId = _user.id,
                    comments = "QUOTATION DELETED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = quotation.id
                });

                if(result && trace > 0)
                    transactionScope.Complete();
                else
                    transactionScope.Dispose();

                return result;
            }
            catch (Exception exception)
            {
                transactionScope.Dispose();
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                throw exception;
            }
        }
    }

    public bool updateStatusByQuotationId(changeStatusModel changeStatus)
    {
        using (TransactionScope transactionScope = new TransactionScope())
        {
            try
            {
                var quotation = _repositoryQuotation.getQuotationById(changeStatus.quotationId);
                if (quotation == null || quotation.status == changeStatus.newStatusId)
                {
                    transactionScope.Dispose();
                    return false;
                }

                var trace = new facadeTrace().addTrace(new traceModel
                {
                    entityType = entityType.QUOTATION,
                    entityId = quotation.id,
                    traceType = traceType.CHANGE_STATUS,
                    userId = changeStatus.userId,
                    comments = changeStatus.comments,
                    beforeChange = JsonConvert.SerializeObject(quotation.status),
                    afterChange = JsonConvert.SerializeObject(changeStatus.newStatusId)
                });

                if (_repositoryQuotation.updateStatusByQuotationById(quotation.id, changeStatus.newStatusId) && trace > 0)
                {
                    transactionScope.Complete();
                    return true;
                }
                else
                {
                    transactionScope.Dispose();
                    return false;
                }
            }
            catch (Exception exception)
            {
                transactionScope.Dispose();
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                return false;
            }
        }
    }

    public quotationModel getQuotationById(int id)
    {
        try
        {
            var quotation = _repositoryQuotation.getQuotationById(id);
            quotation.items = _repositoryQuotation.getQuotationItemsByIdQuotation(id);

            var _repositoryClient = new repositoryClient();
            quotation.client.contactNames = _repositoryClient.getContactNamesByClientId(quotation.client.id);
            quotation.client.contactPhones = _repositoryClient.getContactPhonesByClientId(quotation.client.id);
            return quotation;
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
            return _repositoryQuotation.getQuotationTracesByQuotationId(id);
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
            return _repositoryQuotation.getQuotationTraceById(id);
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
            var quotations = _repositoryQuotation.getQuotationsByTerm(code);
            foreach(var quotation in quotations)
                quotation.items = _repositoryQuotation.getQuotationItemsByIdQuotation(quotation.id);
            return quotations;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}