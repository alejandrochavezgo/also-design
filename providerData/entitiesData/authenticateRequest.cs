namespace providerData.entitiesData;

using System.ComponentModel.DataAnnotations;

public class authenticateRequest
{
    [Required]
    public int id { get; set; }
    [Required]
    public string? username { get; set; }
    [Required]
    public string? email { get; set; }
    [Required]
    public DateTime expirationDate { get; set; }
}