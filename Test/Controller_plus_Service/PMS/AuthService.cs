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

            using (HttpClient client = new HttpClient())
            {
                var loginData = new
                {
                    email = email,
                    password = password
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(loginUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseBody);
                    string jwtToken = jsonResponse["data"]?["jwt"]?.ToString();
                    Console.WriteLine("Login Successful");
                    Console.WriteLine("JWT Token: " + jwtToken);
                    return jwtToken;
                }
                else
                {
                    Console.WriteLine("Login failed: " + response.StatusCode);
                    return null;
                }
            }
        }

        private static (string email, string password) ReadCredentials()
        {
            string json = File.ReadAllText(credentialsFilePath);
            var jsonObject = JObject.Parse(json);
            string email = jsonObject["email"]?.ToString();
            string password = jsonObject["password"]?.ToString();

            return (email, password);
        }
    }
}
