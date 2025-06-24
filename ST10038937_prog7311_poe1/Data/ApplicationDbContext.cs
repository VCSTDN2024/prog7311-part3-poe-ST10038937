using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10038937_prog7311_poe1.Models;

namespace ST10038937_prog7311_poe1.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Farmer> Farmers { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ForumPost> ForumPosts { get; set; } = default!;
    public DbSet<AuditLog> AuditLogs { get; set; } = default!;
        public DbSet<PostReply> PostReplies { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure relationships
        builder.Entity<Farmer>()
            .HasMany(f => f.Products)
            .WithOne(p => p.Farmer)
            .HasForeignKey(p => p.FarmerId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Entity<ApplicationUser>()
            .HasOne<Farmer>()
            .WithOne(f => f.User)
            .HasForeignKey<Farmer>(f => f.UserId);
    }
}
