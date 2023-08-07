namespace MinDatabaseAPI.Services
{
    public class DatabaseConfigurationService
    {
        private readonly string _connectionString;

        public DatabaseConfigurationService()
        {
            _connectionString = File.ReadAllText("database.config");
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
