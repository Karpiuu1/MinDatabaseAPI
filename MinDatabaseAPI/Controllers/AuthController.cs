using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MinDatabaseAPI.Services;
using Microsoft.AspNetCore.Authorization;
using MinDatabaseAPI.Models;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _authService.AuthenticateUserAsync(model.Username, model.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new {token });
        }
    }
}
