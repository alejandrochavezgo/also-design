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

    public bool updateQuotation(quotationModel quotation)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
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