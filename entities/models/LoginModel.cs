using System.ComponentModel;
using Microsoft.Build.Framework;

namespace entities.models
{
    public class LoginModel
    {
        [Required]
        [DisplayName("Username")]
        public string? username { get; set; }
        
        [Required]
        [DisplayName("Password")]
        public string? password { get; set; }
    }
}