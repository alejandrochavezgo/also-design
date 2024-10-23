namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using System.Transactions;

public class facadeUser
{
    private log _logger;
    private repositoryUser _repositoryUser;
    private repositoryEmployee _repositoryEmployee;

    public facadeUser()
    {
        _logger = new log();
        _repositoryUser = new repositoryUser();
        _repositoryEmployee = new repositoryEmployee();
    }

    public List<userModel> getUsers()
    {
        try
        {
            return _repositoryUser.getUsers();
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public userModel getUserById(int id)
    {
        try
        {
            var user = _repositoryUser.getUserById(id);
            user.employee.contactPhones = _repositoryEmployee.getContactPhonesByEmployeeId(user.employee.id);
            return user;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool updateUser(userModel user)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                user.modificationDate = DateTime.Now;
                _repositoryUser.removeContactPhonesByEmployeeId(user.employee.id);

                foreach(var phone in user.employee.contactPhones)
                    if(!string.IsNullOrEmpty(phone))
                        _repositoryUser.addContactPhone(user.employee.id, phone);

                user.email = user.email.Trim().ToUpper();
                user.firstname = user.firstname.Trim().ToUpper();
                user.lastname = user.lastname.Trim().ToUpper();
                user.employee.address = user.employee.address.Trim().ToUpper();
                user.employee.city = user.employee.city.Trim().ToUpper();
                user.employee.state = user.employee.state.Trim().ToUpper();
                user.employee.profession = user.employee.profession.Trim().ToUpper();
                user.employee.jobPosition = user.employee.jobPosition.Trim().ToUpper();

                var result = _repositoryUser.updateUser(user) && _repositoryEmployee.updateEmployee(user.employee);
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

    public bool addUserToEmployee(userModel user)
    {
        try
            {
                user.creationDate = DateTime.Now;
                user.username = user.username.Trim().ToUpper();
                return _repositoryUser.addUserToEmployee(user);
            }
            catch (Exception exception)
            {
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                throw exception;
            }
    }

    public bool addUser(userModel user)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                user.username = user.username.Trim().ToUpper();
                user.firstname = user.firstname.Trim().ToUpper();
                user.lastname = user.lastname.Trim().ToUpper();
                user.creationDate = DateTime.Now;
                user.email = user.email.Trim().ToUpper();
                user.status = user.status;
                user.failCount = 0;
                var userIdAdded = _repositoryUser.addUser(user);
                user.employee.userId = userIdAdded;
                var employeeIdAdded = _repositoryEmployee.addEmployee(user.employee);
                if (user.employee.contactPhones.Count > 0)
                    foreach(var phone in user.employee.contactPhones)
                        if(!string.IsNullOrEmpty(phone))
                            _repositoryEmployee.addContactPhone(employeeIdAdded, phone);

                var result = userIdAdded > 0 && employeeIdAdded > 0;
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