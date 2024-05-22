using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace providerData
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
        }

        public virtual DbSet<ApplicationUser?> Users { get; set; }
    }

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ENUSERS");
            builder.Property(e => e.NormalizedId).HasColumnName("IDUSER");
            builder.Property(e => e.Password).HasColumnName("DSPASSWORD");
            builder.Property(e => e.NormalizedEmail).HasColumnName("DSMAIL");
            builder.Property(e => e.NormalizedUserName).HasColumnName("DSUSERNAME");
            builder.Property(e => e.IsActive).HasColumnName("BOACTIVE");
            builder.Property(e => e.IsLocked).HasColumnName("BOLOCKED");
            builder.Property(e => e.AccessFailedCount).HasColumnName("NUFAILCOUNT");
            builder.Property(e => e.IsPermanentlyDeleted).HasColumnName("BODELETED");
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
}