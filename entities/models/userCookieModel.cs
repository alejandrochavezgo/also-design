namespace entities.models;

public class userCookieModel
{
    public int id { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public List<roleModel>? roles { get; set; }
    public List<menuModel>? menus { get; set; }
}