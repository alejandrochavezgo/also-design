using System.ComponentModel.DataAnnotations;
using entities.enums;

namespace entities.models;

public class traceModel
{
    public int id { get; set; }
    public entityType entityType { get; set; }
    public string? entityTypeDescription { get; set; }
    public traceType traceType { get; set; }
    public string? traceTypeDescription { get; set; }
    public int entityId { get; set; }
    public int userId { get; set; }
    public string? username { get; set; }
    public string? comments { get; set; }
    public string? beforeChange { get; set; }
    public string? afterChange { get; set; }
    public DateTime? creationDate { get; set; }
    public string? creationDateAsString { get; set; }
}