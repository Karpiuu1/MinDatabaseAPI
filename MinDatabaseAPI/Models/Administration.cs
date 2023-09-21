using System.ComponentModel.DataAnnotations;

namespace MinDatabaseAPI.Models
{
    public class Administration
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }

       
    }
}
