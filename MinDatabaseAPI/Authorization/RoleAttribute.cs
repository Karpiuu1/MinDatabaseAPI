using Microsoft.AspNetCore.Authorization;
using System;

namespace MinDatabaseAPI.Authorization
{
    public class RoleAttribute : AuthorizeAttribute
    {
        public RoleAttribute(string role) : base()
        {
            Roles = role;
        }
    }
}
