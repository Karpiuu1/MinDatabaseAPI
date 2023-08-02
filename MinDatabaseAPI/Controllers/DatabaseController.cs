using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/csdatabase")]
    public class DatabaseController : ControllerBase
    {
        private readonly IDatabaseConfigService _databaseConfigService;

        public DatabaseController(IDatabaseConfigService databaseConfigService) 
        {
            _databaseConfigService = databaseConfigService;
        }

        [HttpGet]
        public IActionResult GetConnectionString() 
        {
            string connectionString = _databaseConfigService.GetConnectionString();
            return Ok(connectionString);
        }
    }
}
