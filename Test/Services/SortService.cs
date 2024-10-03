using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Services
{
    public class SortService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> GetSortingJwtToken(string email, string password)
        {
            var loginPayload = new
            {
                email,
                password
            };

            var response = await client.PostAsync("https://api.brand.uat.thehuub.io/authenticate",
                new StringContent(JsonConvert.SerializeObject(loginPayload), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

            var jwtToken = jsonResponse?.data?.jwt;
            if (jwtToken == null)
                throw new Exception("JWT token not found in the response.");

            return jwtToken;
        }

        public static async Task<string> AssignContainerToParcels(string containerId, string packageBarcode, string sortJwtToken)
        {
            string url = $"https://sorting-service.maersk-digital.dev/api/container/{containerId}/parcels";

            var requestBody = new
            {
                destinationCode = "LAX",
                originCode = "AMS",
                destinationSortCenterId = 12,
                originSortCenterId = 201,
                parcelsCodes = new[] { packageBarcode },
                localDateTime = "2024-08-29T18:40:00.531Z",
                statusCountry = "CN",
                sortCenterName = "ALMERE (AMS)",
                routingEnabled = true
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sortJwtToken);

            var response = await client.PatchAsync(url, requestContent);
            response.EnsureSuccessStatusCode(); 

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Assign Container Response: " + responseContent); 
            return responseContent;
        }
    }
}
