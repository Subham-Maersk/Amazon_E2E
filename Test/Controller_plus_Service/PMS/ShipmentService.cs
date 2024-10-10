using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Configuration;

namespace Services
{
    public static class ShipmentService
    {
        private static readonly string shipmentUrl = EnvironmentConfig.GetAllUrls()["PMS_E2E"];

        public static async Task<JObject> CallShipmentApi(string jwtToken, string manifestNumber, string customerIdentifier, string filePath)
        {
            using var client = new HttpClient { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", jwtToken) } };
            
            Console.WriteLine($"Manifest Number: {manifestNumber}");
            Console.WriteLine($"Customer Identifier: {customerIdentifier}");

            var shipmentDataJson = File.ReadAllText(filePath);
            using var multipartFormContent = new MultipartFormDataContent
            {
                { new StringContent(manifestNumber), "number" },
                { new StringContent(customerIdentifier), "customerIdentifier" },
                { new ByteArrayContent(Encoding.UTF8.GetBytes(shipmentDataJson)) { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } }, "file", Path.GetFileName(filePath) }
            };

            var response = await client.PostAsync(shipmentUrl, multipartFormContent);
            return response.IsSuccessStatusCode ? JObject.Parse(await response.Content.ReadAsStringAsync()) : null;
        }

        public static async Task<JObject> ValidateManifestCreation(string jwtToken, string manifestNumber, string customerIdentifier)
        {
            using var client = new HttpClient { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", jwtToken) } };
            var validationUrl = $"{shipmentUrl}?customerIdentifier={customerIdentifier}&number={manifestNumber}";

            var response = await client.GetAsync(validationUrl);
            return response.IsSuccessStatusCode ? JObject.Parse(await response.Content.ReadAsStringAsync()) : null;
        }
    }
}
