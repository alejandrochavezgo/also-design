/*
 * Copyright© 2024 Ideti Web
 * All rights reserved.
 * Total or partial distribution is prohibited.
*/

namespace providerData;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class applicationUser : IdentityUser
{
    public int Id { get; set; }
    public int NormalizedId { get; set; }
    public int AccessFailedCount { get; set; } 
    public string? UserName { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
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
}