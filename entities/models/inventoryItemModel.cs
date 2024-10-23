namespace entities.models;

public class inventoryItemModel
{
    public int id { get; set; }
    public string? itemCode { get; set; }
    public string? itemName { get; set; }
    public int status { get; set; }
    public string? description { get; set; }
    public int material { get; set; }
    public int finishType { get; set; }
    public double? diameter { get; set; }
    public int unitDiameter { get; set; }
    public double? length { get; set; }
    public int unitLength { get; set; }
    public double? weight { get; set; }
    public int unitWeight { get; set; }
    public double? tolerance { get; set; }
    public int unitTolerance { get; set; }
    public int warehouseLocation { get; set; }
    public double? quantity { get; set; }
    public double? reorderQty { get; set; }
    public int unit { get; set; }
    public int? currency { get; set; }
    public decimal? unitValue { get; set; }
    public decimal? totalValue { get; set; }
    public string? notes { get; set; }
    public string? itemDefaultImagePath { get; set; }
    public string? itemImagePath { get; set; }
    public string? bluePrintsPath { get; set; }
    public string? technicalSpecificationsPath { get; set; }
    public DateTime? creationDate { get; set; }
    public DateTime? lastRestockDate { get; set; }
    public DateTime? modificationDate { get; set; }
    public bool itemImageHasNewImage { get; set; }
    public bool itemImageHasOriginalImage { get; set; }
    public bool itemImageHasNotImage { get; set; }
    public bool bluePrintsHasNewDocument { get; set; }
    public bool bluePrintsHasOriginalDocument { get; set; }
    public bool bluePrintsHasNotDocument { get; set; }
    public bool technicalSpecificationsHasNewDocument { get; set; }
    public bool technicalSpecificationsHasOriginalDocument { get; set; }
    public bool technicalSpecificationsHasNotDocument { get; set; }
}