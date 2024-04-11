using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 1,
                Code = "10OFF",
                DiscountAmount = 10,
                MinAmount = 30,
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 2,
                Code = "30OFF",
                DiscountAmount = 30,
                MinAmount = 80,
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 3,
                Code = "50OFF",
                DiscountAmount = 5,
                MinAmount = 15,
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 4,
                Code = "6OFF",
                DiscountAmount = 6,
                MinAmount = 18,
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 5,
                Code = "7OFF",
                DiscountAmount = 7,
                MinAmount = 21,
            });
        }
    }
}