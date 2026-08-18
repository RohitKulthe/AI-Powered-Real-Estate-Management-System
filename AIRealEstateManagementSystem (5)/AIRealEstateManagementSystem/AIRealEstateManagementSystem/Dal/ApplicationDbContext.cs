using AIRealEstateManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AIRealEstateManagementSystem.Dal;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Property> Properties { get; set; }

    public DbSet<Booking> Bookings { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<Inquiry> Inquiries { get; set; }

    public DbSet<PropertyImage> PropertyImages { get; set; }

    public DbSet<EmailOtp> EmailOtps { get; set; }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySQL("server=localhost;port=3306;database=AIRealEstateDB1;user=root;password=Rockstar#2305;");
        }
    }
}


