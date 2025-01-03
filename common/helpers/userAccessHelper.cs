namespace common.helpers;
public class userAccessHelper
{
    public int getIdAccessByAccessDescription(string access)
    {
        try
        {
            var idAccess = 0;
            switch (access)
            {
                case "DASHBOARD":
                    idAccess = 1;
                    break;
                case "CLIENT":
                    idAccess = 8;
                    break;
                case "SUPPLIER":
                    idAccess = 9;
                    break;
                case "PURCHASEORDER":
                    idAccess = 10;
                    break;
                case "QUOTATION":
                    idAccess = 11;
                    break;
                case "WORKORDER":
                    idAccess = 12;
                    break;
                case "INVENTORY":
                    idAccess = 13;
                    break;
                case "USER":
                    idAccess = 14;
                    break;
                case "EMPLOYEE":
                    idAccess = 15;
                    break;
                case "REPORT":
                    idAccess = 16;
                    break;
                case "ENTERPRISE":
                    idAccess = 17;
                    break;
                case "LOGIN":
                    idAccess = 18;
                    break;
                case "SETTING":
                    idAccess = 19;
                    break;
                case "PROJECT":
                    idAccess = 20;
                    break;
                default:
                    idAccess = 0;
                    break;
            }
            return idAccess;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}