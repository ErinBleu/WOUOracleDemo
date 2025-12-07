using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using WouOracleDemo.Models;

namespace WouOracleDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StudentsController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = new List<Student>();

            string connectionString = _config.GetConnectionString("OracleDb");

            using var conn = new OracleConnection(connectionString);
            using var cmd = new OracleCommand("SELECT STUDENT_ID, FIRST_NAME, LAST_NAME, EMAIL, MAJOR, ENROLL_YEAR FROM STUDENTS", conn);

            await conn.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                students.Add(new Student
                {
                    Student_Id = reader.GetInt32(0),
                    First_Name = reader.GetString(1),
                    Last_Name = reader.GetString(2),
                    Email = reader.GetString(3),
                    Major = reader.GetString(4),
                    Enroll_Year = reader.GetInt32(5)
                });
            }

            return Ok(students);
        }
    }
}
