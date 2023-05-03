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
    public class SalePointsController : ControllerBase
    {
        private readonly ILogger<SalePointsController> _logger;
        private readonly IConfiguration Configuration;
        private String connectionString;

        public SalePointsController(ILogger<SalePointsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.Configuration = configuration;
            connectionString = Configuration.GetConnectionString("defaultConnection");

        }

        // GET: api/SalePoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalePoint>>> GetSalePoints()
        {
            return GetAllSalePoints();
        }

        // GET: api/SalePoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalePoint>> GetSalePoint(long id)
        {
            List<SalePoint> salePoints = GetAllSalePoints();
            if (salePoints.Exists(x => x.Id == id))
                return salePoints.Find(x => x.Id == id);
            else return NotFound();
        }

        // PUT: api/SalePoints/5
        [HttpPut("{id}")]
        public async void PutSalePoint(long id, SalePoint salePoint)
        {
            List<SalePoint> salePoints = GetAllSalePoints();
            if (salePoints.Exists(x => x.Id == id &&
            (x.Address != salePoint.Address || x.Description != salePoint.Description)))
                UpdateSalePoint((int)id, salePoint);


        }

        // POST: api/SalePoints
        [HttpPost]
        public void PostSalePoint(SalePoint salePoint)
        {
            List<SalePoint> salePoints = GetAllSalePoints();
            if (!salePoints.Exists(x => x.Id == salePoint.Id))
                AddSalePoint(salePoint);

        }

        // DELETE: api/SalePoints/5
        [HttpDelete("{id}")]
        public async void DeleteSalePoint(long id)
        {
            List<SalePoint> salePoints = GetAllSalePoints();
            if (salePoints.Exists(x => x.Id == id))
                DeleteQuerySalePoint(((int)id));
        }

        private List<SalePoint> GetAllSalePoints()
        {
            String queryString =
                "SELECT * FROM SalePoint";
            List<SalePoint> salePoints = new List<SalePoint>();

            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("defaultConnection")))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SalePoint tempSalePoint = new SalePoint();
                        tempSalePoint.Id = reader.GetInt32(0);
                        tempSalePoint.Address = reader.GetString(1);
                        tempSalePoint.Description = reader.GetString(2);

                        salePoints.Add(tempSalePoint);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка вывода списка точек продаж");
                }
            }

            return salePoints;
        }

        private void AddSalePoint(SalePoint salePoint)
        {
            String queryString =
                "INSERT INTO SalePoint (SalePointID, SalePointAddress, SalePointDescription) "
                + "\n VALUES (@paramId, @paramAddress, @paramDescription)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramId", salePoint.Id);
                cmd.Parameters.AddWithValue("@paramAddress", salePoint.Address);
                cmd.Parameters.AddWithValue("@paramDescription", salePoint.Description);
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

        private void DeleteQuerySalePoint(int id)
        {
            string queryString = "DELETE FROM SalePoint WHERE SalePointID = @paramId ";
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

        private void UpdateSalePoint(int id, SalePoint salePoint)
        {
            String queryString =
            "UPDATE SalePoint SET SalePointDescription = @paramDescription , SalePointAddress = @paramAddress WHERE SalePointID = @ID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramAddress", salePoint.Address);
                cmd.Parameters.AddWithValue("@paramDescription", salePoint.Description);
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
