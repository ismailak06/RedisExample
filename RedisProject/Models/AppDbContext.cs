using Microsoft.EntityFrameworkCore;

namespace RedisProject.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Laptop", Price = 25000m },
                new Product() { Id = 2, Name = "Ram", Price = 500m },
                new Product() { Id = 3, Name = "İşlemci", Price = 2500m },
                new Product() { Id = 4, Name = "Ekran Kartı", Price = 5000m },
                new Product() { Id = 5, Name = "Monitör", Price = 2000m }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}