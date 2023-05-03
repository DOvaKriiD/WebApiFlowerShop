using WebApiFlowerShop_v0._01.FloverShopProvider;
using WebApiFlowerShop_v0._01.Models;

namespace WebApiFlowerShop_v0._01.DataBaseProvider
{
    public class FlowerShopProvider : IFlowerShopProvider
    {
        private FlowerShopСontext _dbContext;
        internal string ConnectionString { get; set; }

        public FlowerShopProvider( string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("Строка подключения должна быть инициализированна.", nameof(connectionString));
            }

            _dbContext = new FlowerShopСontext(connectionString);
        }

        public IEnumerable<SalePoint> GetAllSalePoints()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SalePoint> GetSalePoint(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
