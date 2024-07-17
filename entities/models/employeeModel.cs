namespace entities.models;

public class employeeModel
{
    public int id { get; set;}
    public int userId { get; set;}
    public string? gender { get; set;}
    public string? address { get; set;}
    public string? city { get; set;}
    public string? state { get; set;}
    public string? zipcode { get; set;}
    public string? jobPosition { get; set;}
    public string? profession { get; set;}
    public List<string>? contactPhones { get; set; }
}