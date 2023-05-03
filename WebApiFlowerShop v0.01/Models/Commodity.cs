namespace WebApiFlowerShop_v0._01.Models
{
    public class Commodity
    {
        public int Id { get; set; }
        public string? CommodityName { get; set; }
        public int? price { get; set; }
        public string? BarcodeValue { get; set; }
        public int? CommodityGroupID { get; set; }
    }
}
