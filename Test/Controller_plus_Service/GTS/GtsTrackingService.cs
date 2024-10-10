using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Configuration;

namespace Services
{
    public class GtsTrackingService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<string> GetPackageBarcodeAsync(string gtsToken, string trackingNumber)
        {
            EnvironmentConfig.GetEnvironment("GTS_E2E");

            string apiUrl = $"{EnvironmentConfig.BaseURL}?TrackingNumber={trackingNumber}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gtsToken);

            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var jsonResponse = JToken.Parse(await response.Content.ReadAsStringAsync());
            return jsonResponse.Type switch
            {
                JTokenType.Array => jsonResponse[0]["PackageBarcode"]?.ToString(),
                JTokenType.Object => jsonResponse["PackageBarcode"]?.ToString(),
                _ => throw new Exception("Unexpected JSON format.")
            };
        }
    }
}
