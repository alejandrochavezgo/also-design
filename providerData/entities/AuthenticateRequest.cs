namespace providerData.entities;

using System.ComponentModel.DataAnnotations;

public class AuthenticateRequest
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