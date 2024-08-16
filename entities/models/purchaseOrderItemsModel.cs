namespace entities.models;

public class purchaseOrderItemsModel
{
    public int id { get; set; }
    public int purchaseOrderId { get; set; }
    public string? description { get; set; }
    public string? material { get; set; }
    public string? details { get; set; }
    public string? imagePath { get; set; }
    public string? imageString { get; set; }
    public string? notes { get; set; }
    public bool hasOriginalImage { get; set; }
    public bool hasNewImage { get; set; }
    public int quantity { get; set; }
    public string? unit { get; set; }
    public decimal unitValue { get; set; }
    public decimal totalValue { get; set; }
}