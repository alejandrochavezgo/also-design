namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using System.Transactions;
using entities.enums;
using common.utils;
using common.helpers;

public class facadeProject
{
    private log _logger;
    private userModel _user;
    private facadeTrace _facadeTrace;
    private repositoryProject _repositoryProject;
    private DateTime _dateTime;

    public facadeProject(userModel user)
    {
        _user = user;
        _logger = new log();
        _facadeTrace = new facadeTrace();
        _repositoryProject = new repositoryProject();
        _dateTime = new dateHelper().pstNow();
    }

    public List<List<catalogModel>> getAllProjectCatalogs()
    {
        try
        {
            return new List<List<catalogModel>>(){ _repositoryProject.getStatusTypesCatalog().Where(x => x.id == (int)statusType.ACTIVE || 
                                                                                                    x.id == (int)statusType.INACTIVE || 
                                                                                                    x.id == (int)statusType.LOCKED || 
                                                                                                    x.id == (int)statusType.CLOSED || 
                                                                                                    x.id == (int)statusType.CANCELLED).ToList() };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool addProject(projectModel project)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                project.creationDate = _dateTime;
                var projectIdAdded = _repositoryProject.addProject(project);
                var projectAfter = getProjectById(projectIdAdded);
                var projectSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_PROJECT,
                    entityType = entityType.PROJECT,
                    userId = _user.id,
                    comments = "PROJECT ADDED.",
                    beforeChange = string.Empty,
                    afterChange = JsonConvert.SerializeObject(projectAfter, projectSettings),
                    entityId = projectIdAdded
                });

                var result = projectIdAdded > 0 && trace > 0;
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

    public projectModel getProjectById(int id)
    {
        try
        {
            return _repositoryProject.getProjectById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<projectModel> getProjects()
    {
        try
        {
            return _repositoryProject.getProjects();
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<traceModel> getProjectTracesByProjectId(int id)
    {
        try
        {
            return _repositoryProject.getProjectTracesByProjectId(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public traceModel getProjectTraceById(int id)
    {
        try
        {
            return _repositoryProject.getProjectTraceById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool updateProject(projectModel project)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                var projectBefore = getProjectById(project.id);
                project.modificationDate = _dateTime;
                var result = _repositoryProject.updateProject(project) > 0;
                var projectAfter = getProjectById(project.id);
                var projectSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.UPDATE_PROJECT,
                    entityType = entityType.PROJECT,
                    userId = _user.id,
                    comments = "PROJECT UPDATED.",
                    beforeChange = JsonConvert.SerializeObject(projectBefore, projectSettings),
                    afterChange = JsonConvert.SerializeObject(projectAfter, projectSettings),
                    entityId = project.id
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

    public bool deleteProjectById(int id)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                var project = _repositoryProject.getProjectById(id);
                if (project == null || project.status != (int)statusType.ACTIVE)
                    return false;

                var result = _repositoryProject.deleteProjectById(id);
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.DELETE_PROJECT,
                    entityType = entityType.PROJECT,
                    userId = _user.id,
                    comments = "PROJECT DELETED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = project.id
                });

                result = result && trace > 0;
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

    public List<projectModel> getProjectsByTerm(string name)
    {
        try
        {
            return _repositoryProject.getProjectsByTerm(name);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}