using Microsoft.AspNetCore.Identity;

namespace providerData;

public class ApplicationUser : IdentityUser
{
    public int Id { get; set; }
    public int NormalizedId { get; set; }
    public int AccessFailedCount { get; set; } 
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string EmailConfirmed { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? Password { get; set; }
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public string? NormalizedUserName { get; set; }
    public string? PhoneNumberConfirmed { get; set; }
    public string? SecurityStamp { get; set; }
    public DateTime LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public bool IsPermanentlyDeleted { get; set; }
}