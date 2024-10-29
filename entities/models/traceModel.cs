using System.ComponentModel.DataAnnotations;
using entities.enums;

namespace entities.models;

public class traceModel
{
    public entityType entityType { get; set; }
    public traceType traceType { get; set; }
    public int entityId { get; set; }
    public int userId { get; set; }
    public string? comments { get; set; }
    public string? beforeChange { get; set; }
    public string? afterChange { get; set; }
}