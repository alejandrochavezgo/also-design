namespace entities.models;

public class inventoryListModel
{
    public int? id { get; set; }
    public string? itemName { get; set; }
    public string? itemCode { get; set; }
    public string? itemImagePath { get; set; }
    public string? idQuantityLastRestockDate { get; set; }
}