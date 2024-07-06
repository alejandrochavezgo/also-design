namespace entities.models;

public class clientModel
{
    public int id { get; set; }
    public string? businessName { get; set; }
    public string? rfc { get; set; }
    public string? address { get; set; }
    public string? zipCode { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? country { get; set; }
    public DateTime creationDate { get; set; }
    public string? creationDateAsString { get; set; }
    public DateTime? modificationDate { get; set; }
    public string? modificationDateAsString { get; set; }
    public bool isActive { get; set; }
    public string? statusColor { get; set; }
    public string? statusName { get; set; }
    public List<string>? contactEmails { get; set; }
    public List<string>? contactPhones { get; set; }
}