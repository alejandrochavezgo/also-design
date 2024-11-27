namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using System.Transactions;
using entities.enums;
using common.helpers;
using common.utils;

public class facadeUser
{
    private log _logger;
    private userModel _user;
    private facadeTrace _facadeTrace;
    private repositoryUser _repositoryUser;
    private repositoryEmployee _repositoryEmployee;

    public facadeUser(userModel user)
    {
        _user = user;
        _logger = new log();
        _facadeTrace = new facadeTrace();
        _repositoryUser = new repositoryUser();
        _repositoryEmployee = new repositoryEmployee();
    }

    public List<List<catalogModel>> getAllUserCatalogs()
    {
        try
        {
            var catalogs = new List<List<catalogModel>>();
            catalogs.Add(_repositoryUser.getStatusTypesCatalog().Take(3).ToList());
            catalogs.Add(_repositoryUser.getGenderTypesCatalog());
            catalogs.Add(_repositoryUser.getAccessTypesCatalog());
            catalogs.Add(_repositoryUser.getRoleTypesCatalog());
            return catalogs;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
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

    public userModel getUserByEmailOrUsername(string email, string username)
    {
        try
        {
            return _repositoryUser.getUserByEmailOrUsername(email, username);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public userModel getUserByEmail(string email)
    {
        try
        {
            return _repositoryUser.getUserByEmail(email);
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
            user.userAccess = _repositoryUser.getUserAccessByUserId(user.id);
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
                var facadeEmployee = new facadeEmployee(user);
                var userBefore = getUserById(user.id);
                var employeeBefore = userBefore;
                user.modificationDate = DateTime.Now;
                _repositoryUser.removeContactPhonesByEmployeeId(user.employee.id);

                foreach(var phone in user.employee.contactPhones)
                    if(!string.IsNullOrEmpty(phone))
                        _repositoryUser.addContactPhone(user.employee.id, phone);

                _repositoryUser.removeUserAccessByUserId(user.id);
                var userAccessHelper = new userAccessHelper();
                user.userAccess.AddRange(new List<string> { "DASHBOARD", "ENTERPRISE", "LOGIN", "SETTING" });
                foreach (var access in user.userAccess)
                    if (_repositoryUser.addUserAccess(user.id, userAccessHelper.getIdAccessByAccessDescription(access)) <= 0)
                    {
                        transactionScope.Complete();
                        return false;
                    }

                user.email = user.email.Trim().ToUpper();
                user.firstname = user.firstname.Trim().ToUpper();
                user.lastname = user.lastname.Trim().ToUpper();
                user.employee.address = user.employee.address.Trim().ToUpper();
                user.employee.city = user.employee.city.Trim().ToUpper();
                user.employee.state = user.employee.state.Trim().ToUpper();
                user.employee.profession = user.employee.profession.Trim().ToUpper();
                user.employee.jobPosition = user.employee.jobPosition.Trim().ToUpper();

                var result = true;
                if (!string.IsNullOrEmpty(user.newPasswordHash))
                    result = _repositoryUser.updateUserPassword(user.id, user.newPasswordHash);

                result = result && _repositoryUser.updateUser(user) &&
                                    _repositoryEmployee.updateEmployee(user.employee) &&
                                    _repositoryUser.updateUserRole(user.id, user.userRole) > 0;

                var employeeAfter = getUserById(user.id);
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
                var userSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "status", "password", "passwordHash", "newPasswordHash",
                        "newPassword", "confirmNewPassword", "failCount",
                        "creationDate", "modificationDate", "statusColor",
                        "employee", "userRole"
                    })
                };
                var userAfter = getUserById(user.id);
                var userTrace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.UPDATE_USER,
                    entityType = entityType.USER,
                    userId = _user.id,
                    comments = "USER UPDATED.",
                    beforeChange = JsonConvert.SerializeObject(userBefore, userSettings),
                    afterChange = JsonConvert.SerializeObject(userAfter, userSettings),
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

    public bool addUserToEmployee(userModel user)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                user.creationDate = DateTime.Now;
                user.username = user.username.Trim().ToUpper();
                var result = _repositoryUser.addUserToEmployee(user);
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_USER,
                    entityType = entityType.USER,
                    userId = _user.id,
                    comments = "USER ADDED TO EMPLOYEE.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = user.id
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
                var userIdRoleAdded = _repositoryUser.addUserRole(userIdAdded, user.userRole);
                user.employee.userId = userIdAdded;
                var employeeIdAdded = _repositoryEmployee.addEmployee(user.employee);
                foreach(var phone in user.employee.contactPhones)
                    if(!string.IsNullOrEmpty(phone))
                        if (_repositoryEmployee.addContactPhone(employeeIdAdded, phone) <= 0)
                        {
                            transactionScope.Complete();
                            return false;
                        }
                var userAccessHelper = new userAccessHelper();
                user.userAccess.AddRange(new List<string> { "DASHBOARD", "ENTERPRISE", "LOGIN", "SETTING" });
                foreach (var access in user.userAccess)
                    if (_repositoryUser.addUserAccess(userIdAdded, userAccessHelper.getIdAccessByAccessDescription(access)) <= 0)
                    {
                        transactionScope.Complete();
                        return false;
                    }

                var result = userIdAdded > 0 && employeeIdAdded > 0 && userIdRoleAdded > 0;
                var employeeAfter = new facadeEmployee(user).getEmployeeById(userIdAdded);
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
                    afterChange = JsonConvert.SerializeObject(employeeAfter.employee, employeeSettings),
                    entityId = employeeIdAdded
                });
                var userAfter = getUserById(userIdAdded);
                var userSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "status", "password", "passwordHash", "newPasswordHash",
                        "newPassword", "confirmNewPassword", "failCount",
                        "creationDate", "modificationDate", "statusColor",
                        "employee", "userRole"
                    })
                };
                var userTrace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_USER,
                    entityType = entityType.USER,
                    userId = _user.id,
                    comments = "USER ADDED.",
                    beforeChange = string.Empty,
                    afterChange = JsonConvert.SerializeObject(userAfter, userSettings),
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

    public List<traceModel> getUserTracesByUserId(int id)
    {
        try
        {
            return _repositoryUser.getUserTracesByUserId(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public traceModel getUserTraceById(int id)
    {
        try
        {
            return _repositoryUser.getUserTraceById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool deleteUserById(int id)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                var user = _repositoryUser.getUserById(id);
                if (user == null || user.status != (int)statusType.ACTIVE)
                    return false;

                var result = _repositoryUser.deleteUserById(user.id);
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.DELETE_USER,
                    entityType = entityType.USER,
                    userId = _user.id,
                    comments = "USER DELETED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = user.id
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