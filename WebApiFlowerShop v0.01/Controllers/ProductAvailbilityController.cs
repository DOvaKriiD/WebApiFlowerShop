using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApiFlowerShop_v0._01.Models;

namespace WebApiFlowerShop_v0._01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IConfiguration Configuration;
        private String connectionString;

        public ProductsController(ILogger<ProductsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.Configuration = configuration;
            connectionString = Configuration.GetConnectionString("defaultConnection");

        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductAvailbility>>> GetProducts()
        {
            return GetAllProducts();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductAvailbility>> GetProduct(long id)
        {
            List<ProductAvailbility> Products = GetAllProducts();
            if (Products.Exists(x => x.Id == id))
                return Products.Find(x => x.Id == id);
            else return NotFound();
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async void PutProduct(long id, ProductAvailbility Product)
        {
            List<ProductAvailbility> Products = GetAllProducts();
            if (Products.Exists(x => x.Id == id &&
            (x.CommodityID != Product.CommodityID || x.SalePointID != Product.SalePointID
            || x.Quantity != Product.Quantity)))
                UpdateProduct((int)id, Product);


        }

        // POST: api/Products
        [HttpPost]
        public void PostProduct(ProductAvailbility Product)
        {
            List<ProductAvailbility> Products = GetAllProducts();
            if (!Products.Exists(x => x.Id == Product.Id))
                AddProduct(Product);

        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async void DeleteProduct(long id)
        {
            List<ProductAvailbility> Products = GetAllProducts();
            if (Products.Exists(x => x.Id == id))
                DeleteQueryProduct(((int)id));
        }

        private List<ProductAvailbility> GetAllProducts()
        {
            String queryString =
                "SELECT * FROM ProductAvailbility";
            List<ProductAvailbility> Products = new List<ProductAvailbility>();

            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("defaultConnection")))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductAvailbility tempProduct = new ProductAvailbility();
                        tempProduct.Id = reader.GetInt32(0);
                        tempProduct.CommodityID = reader.GetInt32(1);
                        tempProduct.SalePointID = reader.GetInt32(2);
                        tempProduct.Quantity = reader.GetInt32(3);

                        Products.Add(tempProduct);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка вывода списка точек продаж");
                }
            }

            return Products;
        }

        private void AddProduct(ProductAvailbility Product)
        {
            String queryString =
                "INSERT INTO ProductAvailbility (ProductAvailabilityID, CommodityID, SalePointID, Quantity) "
                + "\n VALUES (@paramId, @paramCommodityID, @paramSalePointID, @paramQuantity)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramId", Product.Id);
                cmd.Parameters.AddWithValue("@paramCommodityID", Product.CommodityID);
                cmd.Parameters.AddWithValue("@paramSalePointID", Product.SalePointID);
                cmd.Parameters.AddWithValue("@paramQuantity", Product.Quantity);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void DeleteQueryProduct(int id)
        {
            string queryString = "DELETE FROM ProductAvailbility WHERE ProductAvailabilityID = @paramId ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramId", id);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void UpdateProduct(int id, ProductAvailbility Product)
        {
            String queryString =
            "UPDATE ProductAvailbility SET CommodityID = @paramCommodityID ," +
            " SalePointID = @paramSalePointID, Quantity =@paramQuantity  WHERE ProductAvailabilityID = @ID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramCommodityID", Product.CommodityID);
                cmd.Parameters.AddWithValue("@paramSalePointID", Product.SalePointID);
                cmd.Parameters.AddWithValue("@paramQuantity", Product.Quantity);
                cmd.Parameters.AddWithValue("@ID", id);
                try
                {
                    connection.Open();
                    int n = cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
