namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

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
                client.contactNames = _repositoryClient.getContactNamesByclientId(client.id);
                client.contactEmails = _repositoryClient.getContactEmailsByclientId(client.id);
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

    public clientModel getClientById(int clientId)
    {
        try
        {
            var client = _repositoryClient.getClientById(clientId);
            client.contactNames = _repositoryClient.getContactNamesByclientId(client.id);
            client.contactEmails = _repositoryClient.getContactEmailsByclientId(client.id);
            client.contactPhones = _repositoryClient.getContactPhonesByClientId(client.id);
            return client;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool addClient(clientModel client)
    {
        try
        {
            client.creationDate = DateTime.Now;
            var clientIdAdded = _repositoryClient.addClient(client);

            if (client.contactNames.Count > 0)
                foreach(var name in client.contactNames)
                    if(!string.IsNullOrEmpty(name))
                        _repositoryClient.addContactName(clientIdAdded, name);

            if (client.contactEmails.Count > 0)
                foreach(var email in client.contactEmails)
                    if(!string.IsNullOrEmpty(email))
                        _repositoryClient.addContactEmail(clientIdAdded, email);

            if (client.contactPhones.Count > 0)
                foreach(var phone in client.contactPhones)
                    if(!string.IsNullOrEmpty(phone))
                        _repositoryClient.addContactPhone(clientIdAdded, phone);

            return clientIdAdded > 0;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool updateClient(clientModel client)
    {
        try
        {
            client.modificationDate = DateTime.Now;
            _repositoryClient.removeContactNamesEmailsAndPhonesByClientId(client.id);

            foreach(var name in client.contactNames)
                if(!string.IsNullOrEmpty(name))
                    _repositoryClient.addContactName(client.id, name);

            foreach(var email in client.contactEmails)
                if(!string.IsNullOrEmpty(email))
                    _repositoryClient.addContactEmail(client.id, email);

            foreach(var phone in client.contactPhones)
                if(!string.IsNullOrEmpty(phone))
                    _repositoryClient.addContactPhone(client.id, phone);

            return _repositoryClient.updateClient(client) > 0;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}