using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Models;
using MinDatabaseAPI.Services;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _authService.AuthenticateUserAsync(model.Username, model.Password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { token });
        }
    }
}

