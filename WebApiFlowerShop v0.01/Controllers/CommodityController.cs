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
    public class CommoditiesController : ControllerBase
    {
        private readonly ILogger<CommoditiesController> _logger;
        private readonly IConfiguration Configuration;
        private String connectionString;

        public CommoditiesController(ILogger<CommoditiesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.Configuration = configuration;
            connectionString = Configuration.GetConnectionString("defaultConnection");

        }

        // GET: api/Commodities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commodity>>> GetCommodities()
        {
            return GetAllCommodities();
        }

        // GET: api/Commodities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Commodity>> GetCommodity(long id)
        {
            List<Commodity> Commodities = GetAllCommodities();
            if (Commodities.Exists(x => x.Id == id))
                return Commodities.Find(x => x.Id == id);
            else return NotFound();
        }

        // PUT: api/Commodities/5
        [HttpPut("{id}")]
        public async void PutCommodity(long id, Commodity Commodity)
        {
            List<Commodity> Commoditys = GetAllCommodities();
            if (Commoditys.Exists(x => x.Id == id &&
            (x.CommodityName != Commodity.CommodityName || x.price != Commodity.price
            || x.BarcodeValue != Commodity.BarcodeValue || x.CommodityGroupID != Commodity.CommodityGroupID)))
                UpdateCommodity((int)id, Commodity);
        }

        // POST: api/Commodities
        [HttpPost]
        public void PostCommodity(Commodity Commodity)
        {
            List<Commodity> Commoditys = GetAllCommodities();
            if (!Commoditys.Exists(x => x.Id == Commodity.Id))
                AddCommodity(Commodity);

        }

        // DELETE: api/Commodities/5
        [HttpDelete("{id}")]
        public async void DeleteCommodity(long id)
        {
            List<Commodity> Commodities = GetAllCommodities();
            if (Commodities.Exists(x => x.Id == id))
                DeleteQueryCommodity(((int)id));
        }

        private List<Commodity> GetAllCommodities()
        {
            String queryString =
                "SELECT * FROM Commodity";
            List<Commodity> Commodities = new List<Commodity>();

            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("defaultConnection")))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Commodity tempCommodity = new Commodity();
                        tempCommodity.Id = reader.GetInt32(0);
                        tempCommodity.CommodityName = reader.GetString(1);
                        tempCommodity.price = reader.GetInt32(2);
                        tempCommodity.BarcodeValue = reader.GetString(3);
                        tempCommodity.CommodityGroupID = reader.GetInt32(4);

                        Commodities.Add(tempCommodity);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка вывода списка точек продаж");
                }
            }

            return Commodities;
        }

        private void AddCommodity(Commodity Commodity)
        {
            String queryString =
                "INSERT INTO Commodity (CommodityID, CommodityName, price,BarcodeValue,CommodityGroupID) "
                + "\n VALUES (@paramId, @paramCommodityName, @paramprice,@paramBarcodeValue, @paramCommodityGroupID)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramId", Commodity.Id);
                cmd.Parameters.AddWithValue("@paramCommodityName", Commodity.CommodityName);
                cmd.Parameters.AddWithValue("@paramprice", Commodity.price);
                cmd.Parameters.AddWithValue("@paramBarcodeValue", Commodity.BarcodeValue);
                cmd.Parameters.AddWithValue("@paramCommodityGroupID", Commodity.CommodityGroupID);
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

        private void DeleteQueryCommodity(int id)
        {
            string queryString = "DELETE FROM Commodity WHERE CommodityID = @paramId ";
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

        private void UpdateCommodity(int id, Commodity Commodity)
        {
            String queryString =
            "UPDATE Commodity SET CommodityName = @paramCommodityName , price = @paramprice, BarcodeValue = @paramBarcodeValue," +
            " CommodityGroupID = @paramCommodityGroupID WHERE CommodityID = @ID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramCommodityName", Commodity.CommodityName);
                cmd.Parameters.AddWithValue("@paramprice", Commodity.price);
                cmd.Parameters.AddWithValue("@paramBarcodeValue", Commodity.BarcodeValue);
                cmd.Parameters.AddWithValue("@CommodityGroupID", Commodity.CommodityGroupID);
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
