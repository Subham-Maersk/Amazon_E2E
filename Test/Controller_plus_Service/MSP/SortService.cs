using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Configuration;

namespace Services
{
    public class SortService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> GetSortingJwtToken(string email, string password)
        {
            var loginPayload = JsonConvert.SerializeObject(new { email, password });
            var response = await client.PostAsync("https://api.brand.uat.thehuub.io/authenticate",
                new StringContent(loginPayload, Encoding.UTF8, "application/json"));
            
            response.EnsureSuccessStatusCode();
            var jwtToken = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())?.data?.jwt;

            return jwtToken ?? throw new Exception("JWT token not found in the response.");
        }

        public static async Task<string> AssignContainerToParcels(string containerId, string packageBarcode, string sortJwtToken)
        {
            EnvironmentConfig.GetEnvironment("MSP_E2E");
            var url = $"{EnvironmentConfig.BaseURL}/{containerId}/parcels"; 

            var requestBody = new
            {
                destinationCode = "LAX",
                originCode = "AMS",
                destinationSortCenterId = 12,
                originSortCenterId = 201,
                parcelsCodes = new[] { packageBarcode },
                localDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                statusCountry = "CN",
                sortCenterName = "ALMERE (AMS)",
                routingEnabled = true
            };

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sortJwtToken);

            var response = await client.PatchAsync(url, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Assign Container Response: " + responseContent);
            return responseContent;
        }
    }
}
