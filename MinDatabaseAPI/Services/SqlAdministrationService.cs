using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MinDatabaseAPI.Models;

namespace MinDatabaseAPI.Services
{
    public class SqlAdministrationService
    {
        private readonly string _connectionString;

        public SqlAdministrationService(DatabaseConfigurationService configService)
        {
            _connectionString = configService.GetConnectionString();
        }

        public int InsertAdministration(Administration administration)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string query = @"
                INSERT INTO Administration (Username, Password, PasswordHash, PasswordSalt, Role)
                VALUES (@Username, @Password, @PasswordHash, @PasswordSalt, @Role);
                SELECT CAST (SCOPE_IDENTITY() AS INT)";
            return db.Query<int>(query, administration).Single();
        }
        public Administration GetAdministrationbyId(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.QuerySingleOrDefault<Administration>("SELECT * FROM Administration WHERE Id = @Id", new { Id = id });
        }
        public Administration GetAdministrationByUsername(string username)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.QuerySingleOrDefault<Administration>("SELECT * FROM Administration WHERE Username = @Username", new { Username = username });
        }
    }
}
