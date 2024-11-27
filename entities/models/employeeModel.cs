namespace entities.models;

public class employeeModel
{
    public int id { get; set;}
    public string? code { get; set;}
    public int userId { get; set;}
    public int status { get; set; }
    public string? statusName { get; set; }
    public string? statusColor { get; set; }
    public int gender { get; set;}
    public string? genderDescription { get; set;}
    public string? address { get; set;}
    public string? city { get; set;}
    public string? state { get; set;}
    public string? zipcode { get; set;}
    public string? jobPosition { get; set;}
    public string? profession { get; set;}
    public string? mainContactPhone { get; set;}
    public List<string>? contactPhones { get; set; }
    public bool hasUser { get; set; }
}