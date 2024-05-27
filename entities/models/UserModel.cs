namespace entities.models;

public class UserModel {
    public int id { get; set; }
    public string? username { get; set; }
    public string? email { get; set; }
    public DateTime? expirationDate { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public string? token { get; set; }
}