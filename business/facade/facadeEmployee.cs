namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using System.Transactions;
using entities.enums;
using common.utils;

public class facadeEmployee
{
    private log _logger;
    private userModel _user;
    private facadeTrace _facadeTrace;
    private repositoryUser _repositoryUser;
    private repositoryEmployee _repositoryEmployee;

    public facadeEmployee(userModel user)
    {
        _user = user;
        _logger = new log();
        _facadeTrace = new facadeTrace();
        _repositoryUser = new repositoryUser();
        _repositoryEmployee = new repositoryEmployee();
    }

    public List<List<catalogModel>> getAllEmployeeCatalogs()
    {
        try
        {
            var catalogs = new List<List<catalogModel>>();
            catalogs.Add(_repositoryEmployee.getStatusTypesCatalog().Take(3).ToList());
            catalogs.Add(_repositoryEmployee.getGenderTypesCatalog());
            return catalogs;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<userModel> getEmployees()
    {
        try
        {
            return _repositoryUser.getNonUsers();
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public userModel getEmployeeById(int id)
    {
        try
        {
            var user = _repositoryUser.getNonUserById(id);
            user.employee.contactPhones = _repositoryEmployee.getContactPhonesByEmployeeId(user.employee.id);
            return user;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool updateEmployee(userModel user)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                var employeeBefore = getEmployeeById(user.id);
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
                var employeeAfter = getEmployeeById(user.id);
                var employeeSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "status", "gender", "statusColor"
                    })
                };
                var employeeTrace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.UPDATE_EMPLOYEE,
                    entityType = entityType.EMPLOYEE,
                    userId = _user.id,
                    comments = "EMPLOYEE UPDATED.",
                    beforeChange = JsonConvert.SerializeObject(employeeBefore, employeeSettings),
                    afterChange = JsonConvert.SerializeObject(employeeAfter, employeeSettings),
                    entityId = user.employee.id
                });
                var userTrace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.UPDATE_USER,
                    entityType = entityType.USER,
                    userId = _user.id,
                    comments = "USER UPDATED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = user.id
                });

                if(result && employeeTrace > 0 && userTrace > 0)
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

    public bool deleteEmployeeById(int id)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                var nonUser = _repositoryUser.getNonUserById(id);
                if (nonUser == null || nonUser.status != (int)statusType.ACTIVE)
                    return false;

                var result = _repositoryEmployee.deleteEmployeeById(nonUser.employee.id);
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.DELETE_EMPLOYEE,
                    entityType = entityType.EMPLOYEE,
                    userId = _user.id,
                    comments = "EMPLOYEE DELETED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = nonUser.id
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

    public bool addEmployee(userModel user)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                user.firstname = user.firstname.Trim().ToUpper();
                user.lastname = user.lastname.Trim().ToUpper();
                user.creationDate = DateTime.Now;
                user.email = user.email.Trim().ToUpper();
                user.status = user.status;
                user.failCount = 0;
                var userIdAdded = _repositoryUser.addNonUser(user);
                user.employee.userId = userIdAdded;
                var employeeIdAdded = _repositoryEmployee.addEmployee(user.employee);
                if (user.employee.contactPhones.Count > 0)
                    foreach(var phone in user.employee.contactPhones)
                        if(!string.IsNullOrEmpty(phone))
                            _repositoryEmployee.addContactPhone(employeeIdAdded, phone);

                var result = userIdAdded > 0 && employeeIdAdded > 0;
                var employeeAfter = getEmployeeById(userIdAdded);
                var employeeSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "status", "gender", "statusColor"
                    })
                };
                var employeeTrace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_EMPLOYEE,
                    entityType = entityType.EMPLOYEE,
                    userId = _user.id,
                    comments = "EMPLOYEE ADDED.",
                    beforeChange = string.Empty,
                    afterChange = JsonConvert.SerializeObject(employeeAfter, employeeSettings),
                    entityId = employeeIdAdded
                });
                var userTrace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_USER,
                    entityType = entityType.USER,
                    userId = _user.id,
                    comments = "USER ADDED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = userIdAdded
                });

                if(result && employeeTrace > 0 && userTrace > 0)
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

    public List<traceModel> getEmployeeTracesByEmployeeId(int id)
    {
        try
        {
            return _repositoryEmployee.getEmployeeTracesByEmployeeId(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public traceModel getEmployeeTraceById(int id)
    {
        try
        {
            return _repositoryEmployee.getEmployeeTraceById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}