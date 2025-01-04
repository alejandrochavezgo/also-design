namespace app.helpers;

using entities.enums;
using entities.models;
using NuGet.Common;

public static class projectFormHelper
{
    public static bool isAddFormValid(projectModel project)
    {
        try
        {
            if (string.IsNullOrEmpty(project.name) || project.client == null || project.client.id <= 0 ||
                string.IsNullOrEmpty(project.description) || project.startDate == null || project.startDate.Value.Date < dateHelper.pstNow().Date ||
                project.endDate == null || project.endDate.Value.Date < dateHelper.pstNow().Date ||
                project.endDate.Value.Date < project.startDate.Value.Date || project.status <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    
    public static bool isUpdateFormValid(projectModel project)
    {
        try
        {
            if (string.IsNullOrEmpty(project.name) || project.client == null || project.client.id <= 0 ||
                string.IsNullOrEmpty(project.description) || project.startDate == null || project.startDate.Value.Date < dateHelper.pstNow().Date ||
                project.endDate == null || project.endDate.Value.Date < dateHelper.pstNow().Date ||
                project.endDate.Value.Date < project.startDate.Value.Date || project.status <= 0 || project.id <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public static bool isUpdateFormValid(projectModel project, bool isStatusChange)
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