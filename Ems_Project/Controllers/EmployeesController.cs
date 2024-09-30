using Ems_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace Ems_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly string connectionString;
        public EmployeesController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }

        [HttpPost]
        public IActionResult CreateEmployee(EmployeeDto employeeDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO Employee" + 
                        "(FirstName, LastName, Gender, Salary) VALUES" +
                        "(@FirstName, @LastName, @Gender, @Salary)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", employeeDto.FirstName);
                        command.Parameters.AddWithValue("@LastName", employeeDto.LastName);
                        command.Parameters.AddWithValue("@Gender", employeeDto.Gender);
                        command.Parameters.AddWithValue("@Salary", employeeDto.Salary);

                        command.ExecuteNonQuery(); 
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Employee", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetEmployee()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Employee";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee();
                                
                                employee.ID = reader.GetInt32(0);
                                employee.FirstName = reader.GetString(1);
                                employee.LastName = reader.GetString(2);
                                employee.Gender = reader.GetString(3);
                                employee.Salary = reader.GetInt32(4);

                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Employee", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            Employee employee = new Employee();
           
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Employee WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                employee.ID = reader.GetInt32(0);
                                employee.FirstName = reader.GetString(1);
                                employee.LastName = reader.GetString(2);
                                employee.Gender = reader.GetString(3);
                                employee.Salary = reader.GetInt32(4);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Employee", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, EmployeeDto employeeDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, Gender = @Gender, " +
                         "Salary = @Salary WHERE ID = @ID";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Firstname", employeeDto.FirstName);
                        command.Parameters.AddWithValue("@Lastname", employeeDto.LastName);
                        command.Parameters.AddWithValue("@Gender", employeeDto.Gender);
                        command.Parameters.AddWithValue("@Salary", employeeDto.Salary);
                        command.Parameters.AddWithValue("@ID", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Employee", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM Employee WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery(); // Check how many rows were affected

                        if (rowsAffected == 0)
                        {
                            // If no rows were affected, return a 404 Not Found
                            return NotFound(new { message = $"Employee with ID {id} not found." });
                        }
                    }

                }
            
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Employee", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok(new { message = "Employee deleted successfully." });
        }

    }
}
