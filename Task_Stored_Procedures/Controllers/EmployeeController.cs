using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Task_Stored_Procedures.Model;

namespace Task_Stored_Procedures.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly string _Configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _Configuration = configuration.GetConnectionString("constr");
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            using (var connection = new SqlConnection(_Configuration))
            {
                var command = new SqlCommand("Sp_For_Emp", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "Create");
                command.Parameters.AddWithValue("@Emp_Id", employee.Id);
                command.Parameters.AddWithValue("@Emp_Name", employee.EmpName);
                command.Parameters.AddWithValue("@Emp_Age", employee.EmpAge);
                command.Parameters.AddWithValue("@Emp_Salary", employee.EmpSalary);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return Ok();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = new List<Employee>();

            using (var connection = new SqlConnection(_Configuration))
            {
                var command = new SqlCommand("Sp_For_Emp", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "SELECT_ALL");

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var employee = new Employee();
                    employee.Id = (int)reader["Emp_Id"];
                    employee.EmpName = (string)reader["Emp_Name"];
                    employee.EmpAge = (int)reader["Emp_Age"];
                    employee.EmpSalary = (int)reader["Emp_Salary"];

                    employees.Add(employee);
                }

                reader.Close();
            }

            return Ok(employees);
        }
        [HttpGet("id:int")]
        public IActionResult Read(int id)
        {
            Employee employee = null;

            using (var connection = new SqlConnection(_Configuration))
            {
                var command = new SqlCommand("Sp_For_Emp", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "Read");
                command.Parameters.AddWithValue("@Emp_Id", id);

                connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    employee = new Employee();
                    employee.Id = (int)reader["Emp_Id"];
                    employee.EmpName = (string)reader["Emp_Name"];
                    employee.EmpAge = (int)reader["Emp_Age"];
                    employee.EmpSalary = (int)reader["Emp_Salary"];
                }

                reader.Close();
            }

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(employee);
            }
        }
        [HttpPut("id:int")]
        public IActionResult Update( Employee employee)
        {
            using (var connection = new SqlConnection(_Configuration))
            {
                var command = new SqlCommand("Sp_For_Emp", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "Update");
                command.Parameters.AddWithValue("@Emp_Id", employee.Id);
                command.Parameters.AddWithValue("@Emp_Name", employee.EmpName);
                command.Parameters.AddWithValue("@Emp_Age", employee.EmpAge);
                command.Parameters.AddWithValue("@Emp_Salary", employee.EmpSalary);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return Ok();
        }
        [HttpDelete]
    public IActionResult Delete(int id)
    {
        using (var connection = new SqlConnection(_Configuration))
        {
            var command = new SqlCommand("Sp_For_Emp", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Action", "Delete");
            command.Parameters.AddWithValue("@Emp_Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        return Ok();
    }



    }
}
