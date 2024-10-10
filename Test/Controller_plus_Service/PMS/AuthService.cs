using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Services
{
    public static class AuthService
    {
        private static readonly string loginUrl = "https://api-eclapi-ncus-sandbox.azurewebsites.net/api/v1/Login";
        private static readonly string credentialsFilePath = Path.Combine("Test_Access_Data_Layer", "credentials.json");

        public static async Task<string> GetJwtToken()
        {
            var (email, password) = ReadCredentials();
            var loginData = new { email, password };

            using var client = new HttpClient();
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(loginUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Login failed: {response.StatusCode}");
                return null;
            }

            var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
            var jwtToken = jsonResponse["data"]?["jwt"]?.ToString();
            Console.WriteLine($"Login Successful\nJWT Token: {jwtToken}");
            return jwtToken;
        }

        private static (string email, string password) ReadCredentials()
        {
            var json = JObject.Parse(File.ReadAllText(credentialsFilePath));
            var email = json["email"]?.ToString();
            var password = json["password"]?.ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Email or password is null or empty.");
            }

            return (email, password);
        }
    }
}
