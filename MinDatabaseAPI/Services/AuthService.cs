using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinDatabaseAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinDatabaseAPI.Services
{
    public class AuthService
    {
        private readonly CustomerDbContext _dbContext;
        private readonly AuthenticationSettings _authenticationSettings;

        public AuthService(CustomerDbContext dbContext, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _authenticationSettings = authenticationSettings;
        }

        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            var user = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Username == username && c.Password == password);

            if (user == null)
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            return token;
        }

        private string GenerateJwtToken(Customer user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role) // Role information
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtKey,
                _authenticationSettings.JwtKey,
                claims,
                expires: expires,
                signingCredentials: creds);



            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

