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
                    if(!(_repositoryQuotation.addQuotationItem(item, quotationIdAdded) > 0))
                        throw new Exception($"Error at saving the quotation item.");

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
}