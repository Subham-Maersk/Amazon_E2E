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
        private readonly HttpClient _httpClient;

        public GtsTrackingService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetPackageBarcodeAsync(string gtsToken, string trackingNumber)
        {
            EnvironmentConfig.GetEnvironment("GTS_E2E");

            string apiUrl = $"{EnvironmentConfig.BaseURL}?TrackingNumber={trackingNumber}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gtsToken);

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                JToken jsonToken = JToken.Parse(jsonResponse);

                if (jsonToken.Type == JTokenType.Array)
                {
                    var firstItem = jsonToken[0];
                    string packageBarcode = firstItem["PackageBarcode"]?.ToString();
                    return packageBarcode;
                }
                else if (jsonToken.Type == JTokenType.Object)
                {
                    string packageBarcode = jsonToken["PackageBarcode"]?.ToString();
                    return packageBarcode;
                }
                else
                {
                    throw new Exception("Unexpected JSON format: neither an array nor an object.");
                }
            }
            else
            {
                throw new Exception($"Error retrieving tracking information: {response.ReasonPhrase}");
            }
        }
    }
}
