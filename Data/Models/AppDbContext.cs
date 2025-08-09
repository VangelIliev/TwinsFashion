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
                .WithMany(s => s.Products)
                .UsingEntity<Dictionary<string, object>>(
            "ProductSize",
            j => j
                .HasOne<Size>()
                .WithMany()
                .HasForeignKey("SizeId")
                .HasConstraintName("FK_ProductSize_Size_SizeId")
                .OnDelete(DeleteBehavior.Cascade),
            j => j
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey("ProductId")
                .HasConstraintName("FK_ProductSize_Product_ProductId")
                .OnDelete(DeleteBehavior.Cascade),
            j =>
            {
                j.HasKey("ProductId", "SizeId");
                j.ToTable("ProductSize");
            });

            // Fix: Set delete behavior to Restrict or NoAction for SubCategory
            modelBuilder.Entity<Product>()
                .HasOne(p => p.SubCategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction
        }
    }
}
