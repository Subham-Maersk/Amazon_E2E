using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Services
{
    public static class FinalMileService
    {
        private static readonly string apiUrl = "https://manifested-label-orchestrator.maersk-digital.dev/api/getfinalmilelabel/";

        public static async Task<JObject> GetFinalMileTracking(string orderId, string apiKey)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);

                string requestUrl = $"{apiUrl}{orderId}";
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(responseBody);
                }
                else
                {
                    Console.WriteLine("Error fetching final mile tracking: " + response.StatusCode);
                    return null;
                }
            }
        }
    }
}
