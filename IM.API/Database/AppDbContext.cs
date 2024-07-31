using Microsoft.EntityFrameworkCore;
using IM.Library.Models;
namespace IM.API.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ShopItem> ShopItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShopItem>(entity =>
            {
                entity.HasKey(e => e.Id); 

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100); 

                entity.Property(e => e.Desc)
                    .HasMaxLength(500); 

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)"); 

                entity.Property(e => e.Amount)
                    .IsRequired(); 

                entity.Property(e => e.IsBogo)
                    .IsRequired(); 
            });
        }
    }
}
