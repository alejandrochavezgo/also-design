using providerData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(common.configurations.ConfigurationManager.AppSettings["connectionStrings:also"]));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddUserStore<ApplicationUserManager>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddHttpClient();
builder.Services.Configure<PasswordHasherOptions>(options =>
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3
);

builder.Services.AddAuthentication(options =>
{
    
});

var app = builder.Build();
app.UseExceptionHandler("/Login/Error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=login}/{action=login}/{id?}");
app.Run();