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
    public class CommodityGroupsController : ControllerBase
    {
        private readonly ILogger<CommodityGroupsController> _logger;
        private readonly IConfiguration Configuration;
        private String connectionString;

        public CommodityGroupsController(ILogger<CommodityGroupsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.Configuration = configuration;
            connectionString = Configuration.GetConnectionString("defaultConnection");

        }

        // GET: api/CommodityGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommodityGroup>>> GetCommodityGroups()
        {
            return GetAllCommodityGroups();
        }

        // GET: api/CommodityGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommodityGroup>> GetCommodityGroup(long id)
        {
            List<CommodityGroup> CommodityGroups = GetAllCommodityGroups();
            if (CommodityGroups.Exists(x => x.Id == id))
                return CommodityGroups.Find(x => x.Id == id);
            else return NotFound();
        }

        // PUT: api/CommodityGroups/5
        [HttpPut("{id}")]
        public async void PutCommodityGroup(long id, CommodityGroup CommodityGroup)
        {
            List<CommodityGroup> CommodityGroups = GetAllCommodityGroups();
            if (CommodityGroups.Exists(x => x.Id == id &&
            (x.GroupName != CommodityGroup.GroupName || x.MinimumWholesaleOrder != CommodityGroup.MinimumWholesaleOrder
            || x.DiscountSize != CommodityGroup.DiscountSize)))
                UpdateCommodityGroup((int)id, CommodityGroup);


        }

        // POST: api/CommodityGroups
        [HttpPost]
        public void PostCommodityGroup(CommodityGroup CommodityGroup)
        {
            List<CommodityGroup> CommodityGroups = GetAllCommodityGroups();
            if (!CommodityGroups.Exists(x => x.Id == CommodityGroup.Id))
                AddCommodityGroup(CommodityGroup);

        }

        // DELETE: api/CommodityGroups/5
        [HttpDelete("{id}")]
        public async void DeleteCommodityGroup(long id)
        {
            List<CommodityGroup> CommodityGroups = GetAllCommodityGroups();
            if (CommodityGroups.Exists(x => x.Id == id))
                DeleteQueryCommodityGroup(((int)id));
        }

        private List<CommodityGroup> GetAllCommodityGroups()
        {
            String queryString =
                "SELECT * FROM CommodityGroup";
            List<CommodityGroup> CommodityGroups = new List<CommodityGroup>();

            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("defaultConnection")))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CommodityGroup tempCommodityGroup = new CommodityGroup();
                        tempCommodityGroup.Id = reader.GetInt32(0);
                        tempCommodityGroup.GroupName = reader.GetString(1);
                        tempCommodityGroup.MinimumWholesaleOrder = reader.GetInt32(2);
                        tempCommodityGroup.DiscountSize = reader.GetInt32(3);

                        CommodityGroups.Add(tempCommodityGroup);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка вывода списка группы товаров");
                }
            }

            return CommodityGroups;
        }

        private void AddCommodityGroup(CommodityGroup CommodityGroup)
        {
            String queryString =
                "INSERT INTO CommodityGroup (CommodityGroupID, GroupName, MinimumWholesaleOrder, DiscountSize) "
                + "\n VALUES (@paramId, @paramGroupName, @paramMinimumWholesaleOrder, @paramDiscountSize)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramId", CommodityGroup.Id);
                cmd.Parameters.AddWithValue("@paramGroupName", CommodityGroup.GroupName);
                cmd.Parameters.AddWithValue("@paramMinimumWholesaleOrder", CommodityGroup.MinimumWholesaleOrder);
                cmd.Parameters.AddWithValue("@paramDiscountSize", CommodityGroup.DiscountSize);
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

        private void DeleteQueryCommodityGroup(int id)
        {
            string queryString = "DELETE FROM CommodityGroup WHERE CommodityGroupID = @paramId ";
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

        private void UpdateCommodityGroup(int id, CommodityGroup CommodityGroup)
        {
            String queryString =
            "UPDATE CommodityGroup SET GroupName = @paramGroupName , MinimumWholesaleOrder = @paramMinimumWholesaleOrder," +
            "DiscountSize = @paramDiscountSize WHERE CommodityGroupID = @ID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramGroupName", CommodityGroup.GroupName);
                cmd.Parameters.AddWithValue("@paramMinimumWholesaleOrder", CommodityGroup.MinimumWholesaleOrder);
                cmd.Parameters.AddWithValue("@paramDiscountSize", CommodityGroup.DiscountSize);
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
