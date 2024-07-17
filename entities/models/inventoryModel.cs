namespace entities.models;

public class inventoryModel
{
    public int id { get; set; }
    public int status { get; set; }
    public string? description { get; set; }
    public string? code { get; set; }
    public int quantity { get; set; }
    public int reorderQuantity { get; set; }
    public int currency { get; set; }
    public string? unit { get; set; }
    public decimal unitValue { get; set; }
    public decimal totalValue { get; set; }
    public string? material { get; set; }
    public DateTime creationDate { get; set; }
    public string? creationDateAsString { get; set; }
    public DateTime? modificationDate { get; set; }
    public string? modificationDateAsString { get; set; }
    public string? statusColor { get; set; }
    public string? statusName { get; set; }
}