namespace entities.models;

public class workOrderModel
{
    public int? id { get; set; }
    public int? quotationId { get; set; }
    public int? priorityId { get; set; }
    public string? rfq { get; set; }
    public int? status { get; set; }
    public List<workOrderItemModel>? items { get; set; }
    public DateTime? deliveryDate { get; set; }
    public string? deliveryDateAsString { get; set; }
    public DateTime? creationDate { get; set; }
    public string? creationDateAsString { get; set; }
    public DateTime? modificationDate { get; set; }
    public string? modificationDateAsString { get; set; }
    public string? statusColor { get; set; }
    public string? statusName { get; set; }
}