namespace entities.models;

public class workOrderItemModel
{
    public int id { get; set; }
    public int workOrderId { get; set; }
    public string? toolNumber { get; set; }
    public int? inventoryItemId { get; set; }
    public string? inventoryItemDescription { get; set; }
    public decimal quantityInStock { get; set; }
    public decimal quantity { get; set; }
    public List<string>? routes { get; set; }
    public string? comments { get; set; }
    public string? statusColor { get; set; }
    public string? statusName { get; set; }
}