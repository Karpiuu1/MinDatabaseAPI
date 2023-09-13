using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinDatabaseAPI.Models;
using MinDatabaseAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MinDatabaseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        public static Administration administration = new Administration();
        private readonly IConfiguration _configuration;
        private readonly SqlAdministrationService _administrationService;


        public AuthController(IConfiguration configuration, SqlAdministrationService administrationService)
        {
            _configuration = configuration;
            _administrationService = administrationService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Administration>> Register (UserDto request)
        {
            try
            {
                var administration = new Administration
                {
                    Username = request.Username,
                    Role = request.Role,
                    Password = request.Password,

                };


                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                administration.PasswordHash = passwordHash;
                administration.PasswordSalt = passwordSalt;
              
                int userId = _administrationService.InsertAdministration(administration);
                administration = _administrationService.GetAdministrationbyId(userId);


                return Ok(administration);
            }
            catch (Exception ex)
            {
                return BadRequest("Error while making new user");
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login (LoginDto request)
        {
            var user = _administrationService.GetAdministrationByUsername(request.Username);

            if (user == null) 
            {
                return BadRequest("User not found.");
            }
            if(!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) 
            {
                return BadRequest("Wrong Password.");
            }
            var token = CreateToken(user);
            return Ok(token);

        }


        private string CreateToken(Administration user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash (string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt)) 
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}

