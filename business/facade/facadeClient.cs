namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using System.Transactions;
using entities.enums;

public class facadeClient
{
    private log _logger;
    private userModel _user;
    private facadeTrace _facadeTrace;
    private repositoryClient _repositoryClient;

    public facadeClient(userModel user)
    {
        _user = user;
        _logger = new log();
        _facadeTrace = new facadeTrace();
        _repositoryClient = new repositoryClient();
    }

    public List<List<catalogModel>> getAllClientCatalogs()
    {
        try
        {
            return new List<List<catalogModel>>(){ _repositoryClient.getStatusTypesCatalog().Take(3).ToList() };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool existClientByBusinessNameAndRfc(string businessName, string rfc)
    {
        try
        {
            var client = _repositoryClient.existClientByBusinessNameAndRfc(businessName.Trim().ToUpper(), rfc.Trim().ToUpper());
            if (client == null || client.id <= 0 || client.status != (int)statusType.ACTIVE)
                return false;

            return true;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool existClientByBusinessNameAndRfcAndId(string businessName, string rfc, int clientId)
    {
        try
        {
            var client = _repositoryClient.existClientByBusinessNameAndRfc(businessName.Trim().ToUpper(), rfc.Trim().ToUpper());
            if (client == null || client.id <= 0 || client.status != (int)statusType.ACTIVE || client.id == clientId)
                return false;

            return true;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<clientModel> getClients()
    {
        try
        {
            var clients = _repositoryClient.getClients();
            foreach(var client in clients)
            {
                client.contactNames = _repositoryClient.getContactNamesByClientId(client.id);
                client.contactEmails = _repositoryClient.getContactEmailsByClientId(client.id);
                client.contactPhones = _repositoryClient.getContactPhonesByClientId(client.id);
            }

            return clients;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public clientModel getClientById(int id)
    {
        try
        {
            var client = _repositoryClient.getClientById(id);
            client.contactNames = _repositoryClient.getContactNamesByClientId(client.id);
            client.contactEmails = _repositoryClient.getContactEmailsByClientId(client.id);
            client.contactPhones = _repositoryClient.getContactPhonesByClientId(client.id);
            return client;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<clientModel> getClientsByTerm(string businessName)
    {
        try
        {
            var clients = _repositoryClient.getClientsByTerm(businessName);
            foreach(var client in clients)
            {
                client.contactNames = _repositoryClient.getContactNamesByClientId(client.id);
                client.contactEmails = _repositoryClient.getContactEmailsByClientId(client.id);
                client.contactPhones = _repositoryClient.getContactPhonesByClientId(client.id);
            }
            return clients;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool addClient(clientModel client)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                client.creationDate = DateTime.Now;
                var clientIdAdded = _repositoryClient.addClient(client);

                if (client.contactNames.Count > 0)
                    foreach(var name in client.contactNames)
                        if(!string.IsNullOrEmpty(name))
                            _repositoryClient.addContactName(clientIdAdded, name.Trim().ToUpper());

                if (client.contactEmails.Count > 0)
                    foreach(var email in client.contactEmails)
                        if(!string.IsNullOrEmpty(email))
                            _repositoryClient.addContactEmail(clientIdAdded, email.Trim().ToUpper());

                if (client.contactPhones.Count > 0)
                    foreach(var phone in client.contactPhones)
                        if(!string.IsNullOrEmpty(phone))
                            _repositoryClient.addContactPhone(clientIdAdded, phone);

                var result = clientIdAdded > 0;
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_CLIENT,
                    entityType = entityType.CLIENT,
                    userId = _user.id,
                    comments = "CLIENT ADDED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = clientIdAdded
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

    public bool updateClient(clientModel client)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                client.modificationDate = DateTime.Now;
                _repositoryClient.removeContactNamesEmailsAndPhonesByClientId(client.id);

                foreach(var name in client.contactNames)
                    if(!string.IsNullOrEmpty(name))
                        _repositoryClient.addContactName(client.id, name.Trim().ToUpper());

                foreach(var email in client.contactEmails)
                    if(!string.IsNullOrEmpty(email))
                        _repositoryClient.addContactEmail(client.id, email.Trim().ToUpper());

                foreach(var phone in client.contactPhones)
                    if(!string.IsNullOrEmpty(phone))
                        _repositoryClient.addContactPhone(client.id, phone);

                var result = _repositoryClient.updateClient(client) > 0;
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.UPDATE_CLIENT,
                    entityType = entityType.CLIENT,
                    userId = _user.id,
                    comments = "CLIENT UPDATED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = client.id
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

    public bool deleteClientById(int id)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                var client = _repositoryClient.getClientById(id);
                if (client == null || client.status != (int)statusType.ACTIVE)
                    return false;

                var result = _repositoryClient.deleteClientById(id);
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.DELETE_CLIENT,
                    entityType = entityType.CLIENT,
                    userId = _user.id,
                    comments = "CLIENT DELETED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = client.id
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
}