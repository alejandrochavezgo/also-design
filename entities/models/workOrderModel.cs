namespace entities.models;

public class workOrderModel
{
    public int id { get; set; }
    public int quotationId { get; set; }
    public int priorityId { get; set; }
    public int userId { get; set; }
    public string? rfq { get; set; }
    public string? projectName { get; set; }
    public string? clientName { get; set; }
    public string? priorityDescription { get; set; }
    public string? quotationPaymentDescription { get; set; }
    public string? quotationCurrencyDescription { get; set; }
    public string? quotationCode { get; set; }
    public string? code { get; set; }
    public decimal? quotationSubtotal { get; set; }
    public decimal? quotationTax { get; set; }
    public decimal? quotationTotal { get; set; }
    public int status { get; set; }
    public List<workOrderItemModel>? items { get; set; }
    public List<quotationItemsModel>? quotationItems { get; set; }
    public DateTime deliveryDate { get; set; }
    public string? deliveryDateAsString { get; set; }
    public DateTime creationDate { get; set; }
    public string? creationDateAsString { get; set; }
    public DateTime? modificationDate { get; set; }
    public string? modificationDateAsString { get; set; }
    public string? statusColor { get; set; }
    public string? statusName { get; set; }
}