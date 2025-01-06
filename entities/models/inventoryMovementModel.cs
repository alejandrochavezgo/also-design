using System.Runtime;
using entities.enums;

namespace entities.models;

public class inventoryMovementModel
{
    public int id { get; set; }
    public inventoryMovementType inventoryMovementType { get; set; }
    public int purchaseOrderId { get; set; }
    public int purchaseOrderItemId { get; set; }
    public int inventoryItemId { get; set; }
    public packingUnitType packingUnitType { get; set; }
    public int userId { get; set; }
    public double quantity { get; set; }
    public packingUnitType unit { get; set; }
    public string? comments { get; set; }
    public string? inventoryItemName { get; set; }
    public string? approvedDeliveredUsername { get; set; }
    public string? receivedUsername { get; set; }
    public string? projectName { get; set; }
    public string? packingUnitTypeDescription { get; set; }
    public string? inventoryMovementTypeDescription { get; set; }
    public DateTime creationDate { get; set; }
    public decimal unitValue { get; set; }
    public decimal totalValue { get; set; }
}