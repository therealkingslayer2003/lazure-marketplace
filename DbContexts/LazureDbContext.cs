using AccountsAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountsAPI.DbContexts
{
    public class LazureDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }


        public LazureDbContext(DbContextOptions<LazureDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users"); // In order to have a correct name mapping
            modelBuilder.Entity<Transaction>().ToTable("transactions");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Category>().ToTable("categories");

            modelBuilder.Entity<User>()
      .HasMany(u => u.Products) // User has many Products
      .WithOne(p => p.User) // Each Product has one User
      .HasForeignKey(p => p.UserId) // Foreign key in Product pointing to User
      .OnDelete(DeleteBehavior.Cascade); // Cascade delete Products when User is deleted

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category) // Each Product has one Category
                .WithMany(c => c.Products) // Category has many Products
                .HasForeignKey(p => p.CategoryId); // Foreign key in Product pointing to Category

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Seller) // Each Transaction has one Seller
                .WithMany(u => u.Sales) // Seller can have many Transactions
                .HasForeignKey(t => t.SellerId) // Foreign key in Transaction pointing to Seller
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete User if related Transactions exist

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Buyer) // Each Transaction has one Buyer
                .WithMany(u => u.Purchases) // Buyer can have many Transactions
                .HasForeignKey(t => t.BuyerId) // Foreign key in Transaction pointing to Buyer
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete User if related Transactions exist

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Product) // Each Transaction has one Product
                .WithMany(p => p.Transactions) // Product can have many Transactions
                .HasForeignKey(t => t.ProductId) // Foreign key in Transaction pointing to Product
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete Product if related Transactions exist

        }

    }
}
