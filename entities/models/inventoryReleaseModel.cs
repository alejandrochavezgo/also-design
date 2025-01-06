using System.ComponentModel.DataAnnotations;

namespace entities.models;

public class inventoryReleaseModel
{
    public int id { get; set; }
    public double stock { get; set; }
    public int receivingUserId { get; set; }
    public int deliveringUserId { get; set; }
    public int projectId { get; set; }
    public double quantityToRelease { get; set; }
    public string? comments { get; set; }
}