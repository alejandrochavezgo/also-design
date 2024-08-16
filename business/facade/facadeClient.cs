namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using System.Transactions;

public class facadeClient
{
    private log _logger;
    private repositoryClient _repositoryClient;

    public facadeClient()
    {
        _logger = new log();
        _repositoryClient = new repositoryClient();
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
                if(result)
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
                if(result)
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