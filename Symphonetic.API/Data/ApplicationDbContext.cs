using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Symphonetic.API.Data;

using Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options) { }

    public DbSet<UserInfo> UserInfos { get; set; }
    
    public DbSet<Project> Projects { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserInfo>(entity => entity.Property(e => e.FirstName).IsRequired());
        builder.Entity<Project>(entity => entity.Property(e => e.Name).IsRequired());
    }

}
