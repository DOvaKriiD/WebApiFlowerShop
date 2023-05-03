using WebApiFlowerShop_v0._01.Models;

namespace WebApiFlowerShop_v0._01.FloverShopProvider
{
    public interface IFlowerShopProvider
    {
        public IEnumerable<SalePoint> GetAllSalePoints();
        public IEnumerable<SalePoint> GetSalePoint(Guid id);
    }
}
