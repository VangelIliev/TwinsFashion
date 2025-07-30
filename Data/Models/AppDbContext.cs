using Microsoft.EntityFrameworkCore;
namespace Data.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        public DbSet<User> Users { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {       
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TwinsFashionDb;Trusted_Connection=True;")            
                .UseSeeding((context, _) =>
                {
                    // Seed Categories
                    if (!context.Set<Category>().Any())
                    {
                        context.Set<Category>().AddRange(
                            new Category { Id = Guid.NewGuid(), Name = "Мъжко" },
                            new Category { Id = Guid.NewGuid(), Name = "Дамско" },
                            new Category { Id = Guid.NewGuid(), Name = "Обувки" }
                        );
                        context.SaveChanges();
                    }
                    // Seed Colors
                    if (!context.Set<Color>().Any())
                    {
                        context.Set<Color>().AddRange(
                            new Color { Id = Guid.NewGuid(), Name = "Червен" },
                            new Color { Id = Guid.NewGuid(), Name = "Син" },
                            new Color { Id = Guid.NewGuid(), Name = "Черен" },
                            new Color { Id = Guid.NewGuid(), Name = "Бял" },
                            new Color { Id = Guid.NewGuid(), Name = "Деним" },
                            new Color { Id = Guid.NewGuid(), Name = "Кафяв" },
                            new Color { Id = Guid.NewGuid(), Name = "Розов" }
                        );
                        context.SaveChanges();
                    }
                });
        }
    }
}
