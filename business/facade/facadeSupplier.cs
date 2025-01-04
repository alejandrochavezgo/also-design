namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using System.Transactions;
using entities.enums;
using common.utils;
using common.helpers;

public class facadeSupplier
{
    private log _logger;
    private userModel _user;
    private facadeTrace _facadeTrace;
    private repositorySupplier _repositorySupplier;
    private DateTime _dateTime;

    public facadeSupplier(userModel user)
    {
        _user = user;
        _logger = new log();
        _facadeTrace = new facadeTrace();
        _repositorySupplier = new repositorySupplier();
        _dateTime = new dateHelper().pstNow();
    }

    public List<List<catalogModel>> getAllSupplierCatalogs()
    {
        try
        {
            return new List<List<catalogModel>>(){ _repositorySupplier.getStatusTypesCatalog().Take(3).ToList() };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool existSupplierByBusinessNameAndRfc(string businessName, string rfc)
    {
        try
        {
            var supplier = _repositorySupplier.existSupplierByBusinessNameAndRfc(businessName.Trim().ToUpper(), rfc.Trim().ToUpper());
            if (supplier == null || supplier.id <= 0 || supplier.status != (int)statusType.ACTIVE)
                return false;

            return true;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool existSupplierByBusinessNameAndRfcAndId(string businessName, string rfc, int supplierId)
    {
        try
        {
            var supplier = _repositorySupplier.existSupplierByBusinessNameAndRfc(businessName.Trim().ToUpper(), rfc.Trim().ToUpper());
            if (supplier == null || supplier.id <= 0 || supplier.status != (int)statusType.ACTIVE || supplier.id == supplierId)
                return false;

            return true;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<supplierModel> getSuppliers()
    {
        try
        {
            var suppliers = _repositorySupplier.getSuppliers();
            foreach(var supplier in suppliers)
            {
                supplier.contactNames = _repositorySupplier.getContactNamesBySupplierId(supplier.id);
                supplier.contactEmails = _repositorySupplier.getContactEmailsBySupplierId(supplier.id);
                supplier.contactPhones = _repositorySupplier.getContactPhonesBySupplierId(supplier.id);
            }

            return suppliers;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public supplierModel getSupplierById(int id)
    {
        try
        {
            var supplier = _repositorySupplier.getSupplierById(id);
            supplier.contactNames = _repositorySupplier.getContactNamesBySupplierId(supplier.id);
            supplier.contactEmails = _repositorySupplier.getContactEmailsBySupplierId(supplier.id);
            supplier.contactPhones = _repositorySupplier.getContactPhonesBySupplierId(supplier.id);
            return supplier;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<supplierModel> getSuppliersByTerm(string businessName)
    {
        try
        {
            var suppliers = _repositorySupplier.getSuppliersByTerm(businessName);
            foreach(var supplier in suppliers)
            {
                supplier.contactNames = _repositorySupplier.getContactNamesBySupplierId(supplier.id);
                supplier.contactEmails = _repositorySupplier.getContactEmailsBySupplierId(supplier.id);
                supplier.contactPhones = _repositorySupplier.getContactPhonesBySupplierId(supplier.id);
            }
            return suppliers;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool addSupplier(supplierModel supplier)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                supplier.creationDate = _dateTime;
                var supplierIdAdded = _repositorySupplier.addSupplier(supplier);

                if (supplier.contactNames.Count > 0)
                    foreach(var name in supplier.contactNames)
                        if(!string.IsNullOrEmpty(name))
                            _repositorySupplier.addContactName(supplierIdAdded, name.Trim().ToUpper());

                if (supplier.contactEmails.Count > 0)
                    foreach(var email in supplier.contactEmails)
                        if(!string.IsNullOrEmpty(email))
                            _repositorySupplier.addContactEmail(supplierIdAdded, email.Trim().ToUpper());

                if (supplier.contactPhones.Count > 0)
                    foreach(var phone in supplier.contactPhones)
                        if(!string.IsNullOrEmpty(phone))
                            _repositorySupplier.addContactPhone(supplierIdAdded, phone);
                
                var result = supplierIdAdded > 0;
                var supplierAfter = getSupplierById(supplierIdAdded);
                var supplierSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_SUPPLIER,
                    entityType = entityType.SUPPLIER,
                    userId = _user.id,
                    comments = "SUPPLIER ADDED.",
                    beforeChange = string.Empty,
                    afterChange = JsonConvert.SerializeObject(supplierAfter, supplierSettings),
                    entityId = supplierIdAdded
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

    public bool updateSupplier(supplierModel supplier)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                var supplierBefore = getSupplierById(supplier.id);
                supplier.modificationDate = _dateTime;
                _repositorySupplier.removeContactNamesEmailsAndPhonesBySupplierId(supplier.id);

                foreach(var name in supplier.contactNames)
                    if(!string.IsNullOrEmpty(name))
                        _repositorySupplier.addContactName(supplier.id, name.Trim().ToUpper());

                foreach(var email in supplier.contactEmails)
                    if(!string.IsNullOrEmpty(email))
                        _repositorySupplier.addContactEmail(supplier.id, email.Trim().ToUpper());

                foreach(var phone in supplier.contactPhones)
                    if(!string.IsNullOrEmpty(phone))
                        _repositorySupplier.addContactPhone(supplier.id, phone);

                var result = _repositorySupplier.updateSupplier(supplier) > 0;
                var supplierAfter = getSupplierById(supplier.id);
                var supplierSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.UPDATE_SUPPLIER,
                    entityType = entityType.SUPPLIER,
                    userId = _user.id,
                    comments = "SUPPLIER UPDATED.",
                    beforeChange = JsonConvert.SerializeObject(supplierBefore, supplierSettings),
                    afterChange = JsonConvert.SerializeObject(supplierAfter, supplierSettings),
                    entityId = supplier.id
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

    public bool deleteSupplierById(int id)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                var supplier = _repositorySupplier.getSupplierById(id);
                if (supplier == null || supplier.status != (int)statusType.ACTIVE)
                    return false;

                var result = _repositorySupplier.deleteSupplierById(id);
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.DELETE_SUPPLIER,
                    entityType = entityType.SUPPLIER,
                    userId = _user.id,
                    comments = "SUPPLIER DELETED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = supplier.id
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

    public List<traceModel> getSupplierTracesBySupplierId(int id)
    {
        try
        {
            return _repositorySupplier.getSupplierTracesBySupplierId(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public traceModel getSupplierTraceById(int id)
    {
        try
        {
            return _repositorySupplier.getSupplierTraceById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}