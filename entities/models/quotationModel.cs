namespace entities.models;

public class quotationModel
{
    public int id { get; set; }
    public int status { get; set; }
    public string? code { get; set; }
    public decimal? subtotal { get; set; }
    public decimal? tax { get; set; }
    public decimal? total { get; set; }
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
}