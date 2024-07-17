namespace entities.models;

public class userModel
{
    public int id { get; set; }
    public int status { get; set; }
    public string? username { get; set; }
    public string? password { get; set; }
    public string? passwordHash { get; set; }
    public string? email { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public int failCount { get; set; }
    public DateTime creationDate { get; set; }
    public string? creationDateAsString { get; set; }
    public DateTime modificationDate { get; set; }
    public string? modificationDateAsString { get; set; }
    public string? statusColor { get; set; }
    public string? statusName { get; set; }
    public employeeModel? employee { get; set; }
}