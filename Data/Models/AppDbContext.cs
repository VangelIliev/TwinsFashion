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

        public DbSet<Size> Sizes { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<AdminUser> AdminUsers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(sc => sc.Category)
            .HasForeignKey(sc => sc.CategoryId);

            modelBuilder.Entity<SubCategory>()
                .HasMany(sc => sc.Products)
                .WithOne(p => p.SubCategory)
                .HasForeignKey(p => p.SubCategoryId);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });


            modelBuilder.Entity<Product>()
                .HasMany(p => p.Sizes)
                .WithMany(s => s.Products);

            // Fix: Set delete behavior to Restrict or NoAction for SubCategory
            modelBuilder.Entity<Product>()
                .HasOne(p => p.SubCategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction
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
                    if(!context.Set<SubCategory>().Any())
                    {
                        context.Set<SubCategory>().AddRange(
                            new SubCategory { Id = Guid.NewGuid(), Name = "Ризи", CategoryId = context.Set<Category>().First(c => c.Name == "Мъжко").Id },
                            new SubCategory { Id = Guid.NewGuid(), Name = "Елегантен Панталон", CategoryId = context.Set<Category>().First(c => c.Name == "Мъжко").Id },
                            new SubCategory { Id = Guid.NewGuid(), Name = "Тениски", CategoryId = context.Set<Category>().First(c => c.Name == "Мъжко").Id },
                            new SubCategory { Id = Guid.NewGuid(), Name = "Спортен Панталон", CategoryId = context.Set<Category>().First(c => c.Name == "Мъжко").Id },
                            new SubCategory { Id = Guid.NewGuid(), Name = "Панталони", CategoryId = context.Set<Category>().First(c => c.Name == "Дамско").Id },
                            new SubCategory { Id = Guid.NewGuid(), Name = "Тениски", CategoryId = context.Set<Category>().First(c => c.Name == "Дамско").Id },
                            new SubCategory { Id = Guid.NewGuid(), Name = "Сака", CategoryId = context.Set<Category>().First(c => c.Name == "Дамско").Id },
                            new SubCategory { Id = Guid.NewGuid(), Name = "Ризи", CategoryId = context.Set<Category>().First(c => c.Name == "Дамско").Id },
                            new SubCategory { Id = Guid.NewGuid(), Name = "Комплекти", CategoryId = context.Set<Category>().First(c => c.Name == "Дамско").Id }
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
                    if (!context.Set<Size>().Any())
                    {
                        context.Set<Size>().AddRange(
                            new Size { Id = Guid.NewGuid(), Name = "XS" },
                            new Size { Id = Guid.NewGuid(), Name = "S" },
                            new Size { Id = Guid.NewGuid(), Name = "M" },
                            new Size { Id = Guid.NewGuid(), Name = "L" },
                            new Size { Id = Guid.NewGuid(), Name = "XL" },
                            new Size { Id = Guid.NewGuid(), Name = "2XL" },
                            new Size { Id = Guid.NewGuid(), Name = "3XL" }
                        );
                        context.SaveChanges();
                    }
                });
        }
    }
}
