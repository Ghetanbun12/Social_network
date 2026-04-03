using Microsoft.EntityFrameworkCore;

using SocialNetwork.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Follow> Follows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Follow>()
        .HasKey(f => new { f.FollowerId, f.FollowingId });

    modelBuilder.Entity<Follow>()
        .HasOne(f => f.Follower)
        .WithMany(u => u.Following)
        .HasForeignKey(f => f.FollowerId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Follow>()
        .HasOne(f => f.Following)
        .WithMany(u => u.Followers)
        .HasForeignKey(f => f.FollowingId)
        .OnDelete(DeleteBehavior.Restrict);

    base.OnModelCreating(modelBuilder);
}
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}