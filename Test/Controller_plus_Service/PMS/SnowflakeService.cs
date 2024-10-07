using System;

namespace Services
{
    public class SnowflakeService
    {
        private readonly string connectionString;

        public SnowflakeService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void LoadData(string query)
        {
            Console.WriteLine($"Executing query: {query}");
        }
    }
}
