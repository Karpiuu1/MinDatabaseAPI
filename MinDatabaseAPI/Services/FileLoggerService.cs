using MinDatabaseAPI.Interface;
using System;
using System.IO;

namespace MinDatabaseAPI.Services
{
    public class FileLoggerService : ILoggerService
    {
        private readonly string logFilePath;

        public FileLoggerService() 
        {
            string baseDirectory = @"D:\Repos\MinDatabaseAPI";
            logFilePath = Path.Combine(baseDirectory, "errors.log");
        }

        public void LogError(string message)
        {
            try
            {
                using (var writer = File.AppendText(logFilePath))
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";

                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error while logging: {ex.Message}");
            }
        }
    }
}
