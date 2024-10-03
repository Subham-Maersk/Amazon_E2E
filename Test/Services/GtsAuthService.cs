using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Utils;

namespace Services
{
    public static class GtsAuthService
    {
        private static readonly string loginUrl = "https://api-oct.maersk.com/api/v1/Login";
        private static readonly HttpClient client = new HttpClient(); 

        public static async Task<string> GetJwtTokenFromGts(string jsonFilePath)
        {
            var credentials = FileUtils.GetCredentials(jsonFilePath);
            string email = credentials["email"].ToString();
            string password = credentials["password"].ToString();

            try
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
                    Console.WriteLine("Login Successful to GTS");
                    Console.WriteLine("GTS JWT Token: " + jwtToken);
                    return jwtToken;
                }
                else
                {
                    Console.WriteLine("Login failed to GTS: " + response.StatusCode);
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e.Message}");
                return null;
            }
        }
    }
}
