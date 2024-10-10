using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Services
{
    public static class FinalMileService
    {
        private static readonly string apiUrl = "https://manifested-label-orchestrator.maersk-digital.dev/api/getfinalmilelabel/";

        public static async Task<JObject> GetFinalMileTracking(string orderId, string apiKey)
        {
            using var client = new HttpClient { DefaultRequestHeaders = { { "ApiKey", apiKey } } };
            var response = await client.GetAsync($"{apiUrl}{orderId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching final mile tracking: {response.StatusCode}");
                return null;
            }

            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }
    }
}
