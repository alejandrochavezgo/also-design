using System.ComponentModel.DataAnnotations;

namespace entities.models;

public class projectModel
{
    public int id { get; set; }
    public int status { get; set; }
    public string? name { get; set; }
    public clientModel? client { get; set; }
    public string? description { get; set; }
    public DateTime? startDate { get; set; }
    public string? startDateAsString { get; set; }
    public DateTime? endDate { get; set; }
    public string? endDateAsString { get; set; }
    public DateTime creationDate { get; set; }
    public string? creationDateAsString { get; set; }
    public DateTime? modificationDate { get; set; }
    public string? modificationDateAsString { get; set; }
    public string? statusColor { get; set; }
    public string? statusName { get; set; }
}