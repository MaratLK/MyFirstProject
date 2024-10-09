using Microsoft.EntityFrameworkCore;
using PLK_TwoTry_Back.Models;

namespace PLKTransit.Data
{
    public class PLKTransitContext : DbContext
    {
        public PLKTransitContext(DbContextOptions<PLKTransitContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .Property(u => u.DateRegistered)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleID);

            // Связь News и NewsImage с каскадным удалением
            modelBuilder.Entity<News>()
                .HasMany(n => n.NewsImages)
                .WithOne(ni => ni.News)
                .HasForeignKey(ni => ni.NewsID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
    