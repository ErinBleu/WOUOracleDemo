using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace WouOracleDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestOracleController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TestOracleController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            var connString = _config.GetConnectionString("OracleDb");

            try
            {
                using var conn = new OracleConnection(connString);
                await conn.OpenAsync();

                return Ok(new
                {
                    message = "Oracle connection opened successfully",
                    state = conn.State.ToString()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Failed to open Oracle connection",
                    exception = ex.Message
                });
            }
        }
    }
}
