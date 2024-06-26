namespace entities.models;

public class userModel
{
    public int id { get; set; }
    public string? username { get; set; }
    public string? email { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public bool isActive { get; set; }
    public bool isLocked { get; set; }
    public string? creationDate { get; set; }
    public string? modificationDate { get; set; }
    public string? statusColor { get; set; }
    public string? statusName { get; set; }
}