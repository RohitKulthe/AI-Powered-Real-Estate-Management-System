using AIRealEstateManagementSystem.Dal;
using AIRealEstateManagementSystem.Models;
using AIRealEstateManagementSystem.Models.Setting;
using AIRealEstateManagementSystem.Services;
using AIRealEstateManagementSystem.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// Add MVC
// ===============================
builder.Services.AddControllersWithViews();

// ===============================
// Session Configuration
// ===============================
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ===============================
// Register DbContext
// ===============================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ===============================
// Register Identity
// ===============================
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ===============================
// Email Settings
// ===============================
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// ===============================
// Register Services
// ===============================
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();


// =========================================
// Create Roles & Assign Admin Automatically
// =========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var configuration = services.GetRequiredService<IConfiguration>();

    string? adminEmail = configuration["AdminSettings:Email"];

    // Create Admin Role
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Create Customer Role
    if (!await roleManager.RoleExistsAsync("Customer"))
    {
        await roleManager.CreateAsync(new IdentityRole("Customer"));
    }

    // Create Seller Role
    if (!await roleManager.RoleExistsAsync("Seller"))
    {
        await roleManager.CreateAsync(new IdentityRole("Seller"));
    }

    // Assign Admin Role
    if (!string.IsNullOrWhiteSpace(adminEmail))
    {
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser != null)
        {
            if (await userManager.IsInRoleAsync(adminUser, "Customer"))
            {
                await userManager.RemoveFromRoleAsync(adminUser, "Customer");
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}

// ===============================
// Configure HTTP Pipeline
// ===============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Enable Session
app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();