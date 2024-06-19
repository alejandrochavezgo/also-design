namespace entities.models;

using System.ComponentModel;
using Microsoft.Build.Framework;

public class loginModel
{
    [Required]
    [DisplayName("Username")]
    public string? username { get; set; }
    
    [Required]
    [DisplayName("Password")]
    public string? password { get; set; }
}