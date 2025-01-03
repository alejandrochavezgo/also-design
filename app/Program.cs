using providerData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.LoginPath = "/login/login";
                    options.AccessDeniedPath = "/login/accessDenied";
                });

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<applicationDbContext>(options => options.UseSqlServer(common.configurations.configurationManager.appSettings["connectionStrings:also"]));
builder.Services.AddIdentity<applicationUser, IdentityRole>()
                .AddUserStore<applicationUserManager>()
                .AddEntityFrameworkStores<applicationDbContext>();
builder.Services.AddHttpClient();
builder.Services.Configure<PasswordHasherOptions>(options =>
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3
);

var app = builder.Build();
app.UseExceptionHandler("/login/accessDenied");
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