namespace providerData.entitiesData;

using entities.models;

public class userModel
{
    public int id { get; set; }
    public string? username { get; set; }
    public string? email { get; set; }
    public DateTime? expirationDate { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public List<roleModel>? roles { get; set; }
    public List<menuModel>? menus { get; set; }
    public string? token { get; set; }
}