using Microsoft.EntityFrameworkCore;

namespace WebApiFlowerShop_v0._01.Models
{
    public class FlowerShopСontext : DbContext
    {
        private string ConnectionString { get; set; }
        public FlowerShopСontext(DbContextOptions<FlowerShopСontext> options)
            : base(options)
        {
        }

        public FlowerShopСontext(string connectionString)
        {
            ConnectionString = connectionString;
        }



        public DbSet<SalePoint> SalePoints { get; set; } = null!;

            
    }
}
