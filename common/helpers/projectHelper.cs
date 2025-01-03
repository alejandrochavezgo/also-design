namespace common.helpers;

using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

public class projectFormHelper
{
    private log _logger;

    public projectFormHelper ()
    {
        _logger = new log();
    }

    public bool isAddFormValid(projectModel project)
    {
        try
        {
            if (string.IsNullOrEmpty(project.name) || project.client == null || project.client.id <= 0 ||
                string.IsNullOrEmpty(project.description) || project.startDate == null || project.startDate.Value.Date < DateTime.Now.Date ||
                project.endDate == null || project.endDate.Value.Date < DateTime.Now.Date ||
                project.endDate.Value.Date < project.startDate.Value.Date || project.status <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool isUpdateFormValid(projectModel project)
    {
        try
        {
            if (string.IsNullOrEmpty(project.name) || project.client == null || project.client.id <= 0 ||
                string.IsNullOrEmpty(project.description) || project.startDate == null || project.startDate.Value.Date < DateTime.Now.Date ||
                project.endDate == null || project.endDate.Value.Date < DateTime.Now.Date ||
                project.endDate.Value.Date < project.startDate.Value.Date || project.status <= 0 || project.id <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public bool isUpdateFormValid(projectModel project, bool isStatusChange)
    {
        try
        {
            if (project == null || project.id <= 0 || project.status != (int)statusType.ACTIVE)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}