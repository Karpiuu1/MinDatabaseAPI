using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using MinDatabaseAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MinDatabaseAPI.Models;

namespace MinDatabaseAPI.Services
{
    public class AuthService
    {
        private readonly CustomerDbContext _dbContext;
        private readonly IConfiguration _configuration;


        public AuthService(CustomerDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
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
                new Claim(ClaimTypes.Role, user.Role),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
