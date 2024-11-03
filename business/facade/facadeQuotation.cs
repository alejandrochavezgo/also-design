namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using entities.enums;
using Newtonsoft.Json;
using System.Transactions;

public class facadeQuotation
{
    private log _logger;
    private repositoryQuotation _repositoryQuotation;

    public facadeQuotation()
    {
        _logger = new log();
        _repositoryQuotation = new repositoryQuotation();
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
                quotation.creationDate = DateTime.Now;
                var quotationIdAdded = _repositoryQuotation.addQuotation(quotation);

                foreach (var item in quotation.items)
                {
                    item.imagePath = string.IsNullOrEmpty(item.imagePath) ? string.Empty : item.imagePath;
                    if(!(_repositoryQuotation.addQuotationItem(item, quotationIdAdded) > 0))
                        throw new Exception($"Error at saving the quotation item.");
                }

                transactionScope.Complete();
                return true;
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
                var quotationFromDatabase = _repositoryQuotation.getQuotationById(quotation.id);
                if (quotationFromDatabase != null && quotationFromDatabase.status != (int)statusType.ACTIVE)
                {
                    transactionScope.Dispose();
                    return false;
                }

                quotation.modificationDate = DateTime.Now;
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

                transactionScope.Complete();
                return true;
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
        try
        {
            var quotation = _repositoryQuotation.getQuotationById(id);
            if (quotation == null || quotation.status != (int)statusType.ACTIVE)
                return false;
            return _repositoryQuotation.deleteQuotationById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
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
                    beforeChange = $"{quotation.status}",
                    afterChange = $"{changeStatus.newStatusId}"
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
}