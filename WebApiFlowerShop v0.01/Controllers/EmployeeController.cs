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
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IConfiguration Configuration;
        private String connectionString;

        public EmployeesController(ILogger<EmployeesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.Configuration = configuration;
            connectionString = Configuration.GetConnectionString("defaultConnection");

        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return GetAllEmployees();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(long id)
        {
            List<Employee> Employees = GetAllEmployees();
            if (Employees.Exists(x => x.Id == id))
                return Employees.Find(x => x.Id == id);
            else return NotFound();
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async void PutEmployee(long id, Employee Employee)
        {
            List<Employee> Employees = GetAllEmployees();
            if (Employees.Exists(x => x.Id == id &&
            (x.EmployeeName != Employee.EmployeeName || x.SellPointId != Employee.SellPointId)))
                UpdateEmployee((int)id, Employee);


        }

        // POST: api/Employees
        [HttpPost]
        public void PostEmployee(Employee Employee)
        {
            List<Employee> Employees = GetAllEmployees();
            if (!Employees.Exists(x => x.Id == Employee.Id))
                AddEmployee(Employee);

        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async void DeleteEmployee(long id)
        {
            List<Employee> Employees = GetAllEmployees();
            if (Employees.Exists(x => x.Id == id))
                DeleteQueryEmployee(((int)id));
        }

        private List<Employee> GetAllEmployees()
        {
            String queryString =
                "SELECT * FROM Employee";
            List<Employee> Employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("defaultConnection")))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee tempEmployee = new Employee();
                        tempEmployee.Id = reader.GetInt32(0);
                        tempEmployee.EmployeeName = reader.GetString(1);
                        tempEmployee.SellPointId = reader.GetInt32(2);

                        Employees.Add(tempEmployee);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка вывода списка работников");
                }
            }

            return Employees;
        }

        private void AddEmployee(Employee Employee)
        {
            String queryString =
                "INSERT INTO Employee (EmployeeID, EmployeeFullName, SellPointID) "
                + "\n VALUES (@paramId, @EmployeeFullName, @paramSellPointID)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramId", Employee.Id);
                cmd.Parameters.AddWithValue("@EmployeeFullName", Employee.EmployeeName);
                cmd.Parameters.AddWithValue("@paramSellPointID", Employee.SellPointId);
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

        private void DeleteQueryEmployee(int id)
        {
            string queryString = "DELETE FROM Employee WHERE EmployeeID = @paramId ";
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

        private void UpdateEmployee(int id, Employee Employee)
        {
            String queryString =
            "UPDATE Employee SET SellPointID = @paramSellPointID , EmployeeFullName = @paramEmployeeFullName WHERE EmployeeID = @ID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Parameters.AddWithValue("@paramEmployeeFullName", Employee.EmployeeName);
                cmd.Parameters.AddWithValue("@paramSellPointID", Employee.SellPointId  );
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
