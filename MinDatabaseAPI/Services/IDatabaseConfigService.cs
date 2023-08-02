namespace MinDatabaseAPI.Services
{                                                                   // serwis do odczytywania connectionstringa
    public interface IDatabaseConfigService
    {
        string GetConnectionString();
    }

    public class DatabaseConfigService : IDatabaseConfigService
    {
        private readonly string _configFilePath;

        public DatabaseConfigService()
        {
            _configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "database.config");
        }

        public string GetConnectionString()
        {
            if (File.Exists(_configFilePath))
            {
                return File.ReadAllText(_configFilePath);
            }

            throw new FileNotFoundException("Database configuration file not found.");
        }
    }
}
