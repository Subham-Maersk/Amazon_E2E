using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Services
{
    public static class AuthService
    {
        private static readonly string loginUrl = "https://api-eclapi-ncus-sandbox.azurewebsites.net/api/v1/Login";

        public static async Task<string> GetJwtToken(string email, string password)
        {
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
                    Console.WriteLine("Login Successful :JWT Token is " + jwtToken);
                    return jwtToken;
                }
                else
                {
                    Console.WriteLine("Login failed: " + response.StatusCode);
                    return null;
                }
            }
        }
    }
}
