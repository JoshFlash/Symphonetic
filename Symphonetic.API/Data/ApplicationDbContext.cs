using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Symphonetic.API.Models;

namespace Symphonetic.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options) { }

    public DbSet<UserInfo> UserInfos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Define UserInfo entity
        builder.Entity<UserInfo>(entity =>
        {
            // Entity configuration goes here
            entity.Property(e => e.FirstName).IsRequired();
        });
    }

}
