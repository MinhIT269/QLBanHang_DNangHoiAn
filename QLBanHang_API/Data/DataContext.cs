using Microsoft.EntityFrameworkCore;
using PBL6_QLBH.Models;

namespace PBL6_QLBH.Data
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }



        public DbSet<Video> Videos { get; set; }  //Dbset<T> tham số T là 1 loại thực thể
        public DbSet<Location> Locations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }


        /*   protected override void OnModelCreating(ModelBuilder modelBuilder)
           {
               // Cấu hình quan hệ một-một giữa User và UserInfo
               modelBuilder.Entity<User>()
                   .HasOne(u => u.UserInfo)
                   .WithOne(ui => ui.User)
                   .HasForeignKey<UserInfo>(ui => ui.UserId);

               // Cấu hình quan hệ một-nhiều giữa Brand và Product
               modelBuilder.Entity<Brand>()
                   .HasMany(b => b.Products)
                   .WithOne(p => p.Brand)
                   .HasForeignKey(p => p.BrandId);

               // Cấu hình quan hệ một-nhiều giữa Brand và Location
               modelBuilder.Entity<Brand>()
                   .HasMany(b => b.Locations)
                   .WithOne(l => l.Brand)
                   .HasForeignKey(l => l.BrandId);

               // Cấu hình quan hệ một-nhiều giữa Cart và CartItem
               modelBuilder.Entity<Cart>()
                   .HasMany(c => c.CartItems)
                   .WithOne(ci => ci.Cart)
                   .HasForeignKey(ci => ci.CartId);

               // Cấu hình quan hệ một-nhiều giữa Order và OrderDetail
               modelBuilder.Entity<Order>()
                   .HasMany(o => o.OrderDetails)
                   .WithOne(od => od.Order)
                   .HasForeignKey(od => od.OrderId);

               // Cấu hình quan hệ một-nhiều giữa Product và ProductImage
               modelBuilder.Entity<Product>()
                   .HasMany(p => p.ProductImages)
                   .WithOne(pi => pi.Product)
                   .HasForeignKey(pi => pi.ProductId);

               // Cấu hình quan hệ một-nhiều giữa Product và Review
               modelBuilder.Entity<Product>()
                   .HasMany(p => p.Reviews)
                   .WithOne(r => r.Product)
                   .HasForeignKey(r => r.ProductId);

               // Cấu hình quan hệ nhiều-nhiều giữa Product và Category thông qua ProductCategory
               modelBuilder.Entity<ProductCategory>()
                   .HasKey(pc => new { pc.ProductId, pc.CategoryId });

               modelBuilder.Entity<ProductCategory>()
                   .HasOne(pc => pc.Product)
                   .WithMany(p => p.ProductCategories)
                   .HasForeignKey(pc => pc.ProductId);

               modelBuilder.Entity<ProductCategory>()
                   .HasOne(pc => pc.Category)
                   .WithMany(c => c.ProductCategories)
                   .HasForeignKey(pc => pc.CategoryId);

               // Cấu hình quan hệ một-nhiều giữa PaymentMethod và PaymentDetail
               modelBuilder.Entity<PaymentMethod>()
                   .HasMany(pm => pm.PaymentDetails)
                   .WithOne(pd => pd.PaymentMethod)
                   .HasForeignKey(pd => pd.PaymentMethodId);

               // Cấu hình quan hệ một-nhiều giữa User và PaymentDetail
               modelBuilder.Entity<User>()
                   .HasMany(u => u.PaymentDetails)
                   .WithOne(pd => pd.User)
                   .HasForeignKey(pd => pd.UserId);

               // Cấu hình quan hệ một-nhiều giữa User và Order
               modelBuilder.Entity<User>()
                   .HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId);

               // Cấu hình quan hệ một-nhiều giữa User và Review
               modelBuilder.Entity<User>()
                   .HasMany(u => u.Reviews)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId);

               // Cấu hình quan hệ một-nhiều giữa Promotion và Order
               modelBuilder.Entity<Promotion>()
                   .HasMany(p => p.Orders)
                   .WithOne(o => o.Promotion)
                   .HasForeignKey(o => o.PromotionId);

               // Cấu hình quan hệ một-nhiều giữa Video và Product
               modelBuilder.Entity<Video>()
                   .HasMany(v => v.Products)
                   .WithOne(p => p.Video)
                   .HasForeignKey(p => p.VideoId);

               // Cấu hình quan hệ một-nhiều giữa Role và User
               modelBuilder.Entity<Role>()
                   .HasMany(r => r.Users)
                   .WithOne(u => u.Role)
                   .HasForeignKey(u => u.RoleId);

               // Cấu hình quan hệ một-một giữa Transaction và Order
               modelBuilder.Entity<Transaction>()
                   .HasOne(t => t.Order)
                   .WithOne(o => o.Transaction)
                   .HasForeignKey<Transaction>(t => t.OrderId);

               // Cấu hình quan hệ một-nhiều giữa Transaction và PaymentMethod
               modelBuilder.Entity<Transaction>()
                   .HasOne(t => t.PaymentMethod)
                   .WithMany(pm => pm.Transactions)
                   .HasForeignKey(t => t.PaymentMethodId);

               base.OnModelCreating(modelBuilder);
           }*/
    }
}
