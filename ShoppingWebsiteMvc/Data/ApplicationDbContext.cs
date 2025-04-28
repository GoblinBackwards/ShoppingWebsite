using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsiteMvc.Models;

namespace ShoppingWebsiteMvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomerIdentityUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreItem>().Property(t => t.PriceGBP).HasPrecision(10, 2);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<StoreItem> StoreItems { get; set; }
    }
}
