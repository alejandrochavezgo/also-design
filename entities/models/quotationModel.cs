using System.Drawing.Printing;
using entities.enums;

namespace entities.models;

public class quotationModel
{
    public int id { get; set; }
    public int projectId { get; set; }
    public string? projectName { get; set; }
    public int status { get; set; }
    public string? code { get; set; }
    public decimal? subtotal { get; set; }
    public decimal? taxRate { get; set; }
    public decimal? taxAmount { get; set; }
    public decimal? totalAmount { get; set; }
    public DateTime creationDate { get; set; }
    public string? creationDateAsString { get; set; }
    public DateTime? modificationDate { get; set; }
    public string? modificationDateAsString { get; set; }
    public string? statusColor { get; set; }
    public string? statusName { get; set; }
    public clientModel? client { get; set; }
    public userModel? user { get; set; }
    public paymentModel? payment { get; set; }
    public currencyModel? currency { get; set; }
    public string? generalNotes { get; set; }
    public List<quotationItemsModel>? items { get; set; }
}