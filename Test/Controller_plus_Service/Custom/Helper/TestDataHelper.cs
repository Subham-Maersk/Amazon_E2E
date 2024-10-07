using System;

namespace TestProject.Helpers
{
    public class TestDataHelper
    {
      
        public static string GenerateRandomEmail()
        {
            var random = new Random();
            var uniqueId = random.Next(1000, 9999);
            return $"user{uniqueId}@example.com";
        }

        public static string GenerateRandomFirstName()
        {
            var firstNames = new[] { "John", "Jane", "Michael", "Sara", "Chris" };
            var random = new Random();
            return firstNames[random.Next(firstNames.Length)];
        }

        public static string GenerateRandomLastName()
        {
            var lastNames = new[] { "Smith", "Doe", "Johnson", "Brown", "Williams" };
            var random = new Random();
            return lastNames[random.Next(lastNames.Length)];
        }

        public static string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }
    }
}
