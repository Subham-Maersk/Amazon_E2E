using System;
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
            var loginData = new
            {
                email = credentials["email"].ToString(),
                password = credentials["password"].ToString()
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync(loginUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
                    return jsonResponse["data"]?["jwt"]?.ToString();
                }
                return null;
            }
            catch (HttpRequestException) { return null; }
            catch (Exception) { return null; }
        }
    }
}
