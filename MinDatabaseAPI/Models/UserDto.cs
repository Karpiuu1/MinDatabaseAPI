﻿using System.ComponentModel.DataAnnotations;

namespace MinDatabaseAPI.Models
{
    public class UserDto
    {
   
        [Required]
        public string Username { get; set; } = string.Empty;
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
