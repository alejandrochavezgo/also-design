﻿/*
 * Copyright© 2024 Ideti Web
 * All rights reserved.
 * Total or partial distribution is prohibited.
*/

namespace providerData;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class applicationDbContext : IdentityDbContext<applicationUser>
{
    public applicationDbContext(DbContextOptions<applicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
    }

    public virtual DbSet<applicationUser?> users { get; set; }
}

public class ApplicationUserConfiguration : IEntityTypeConfiguration<applicationUser>
{
    public void Configure(EntityTypeBuilder<applicationUser> builder)
    {
        builder.ToTable("USERS");
        builder.Property(e => e.Id).HasColumnName("IDUSER");
        builder.Property(e => e.NormalizedId).HasColumnName("IDUSER");
        builder.Property(e => e.Password).HasColumnName("PASSWORD");
        builder.Property(e => e.NormalizedEmail).HasColumnName("EMAIL");
        builder.Property(e => e.NormalizedUserName).HasColumnName("USERNAME");
        builder.Property(e => e.Firstname).HasColumnName("FIRSTNAME");
        builder.Property(e => e.Lastname).HasColumnName("LASTNAME");
        builder.Property(e => e.StatusId).HasColumnName("IDSTATUS");
        builder.Property(e => e.AccessFailedCount).HasColumnName("FAILCOUNT");
        builder.Ignore(e => e.UserName);
        builder.Ignore(e => e.EmailConfirmed);
        builder.Ignore(e => e.Email);
        builder.Ignore(e => e.PasswordHash);
        builder.Ignore(e => e.PhoneNumber);
        builder.Ignore(e => e.ConcurrencyStamp);
        builder.Ignore(e => e.PhoneNumberConfirmed);
        builder.Ignore(e => e.SecurityStamp);
        builder.Ignore(e => e.LockoutEnd);
        builder.Ignore(e => e.LockoutEnabled);
        builder.Ignore(e => e.TwoFactorEnabled);
        builder.Ignore(e => e.Id);
        builder.HasNoKey();
    }
}