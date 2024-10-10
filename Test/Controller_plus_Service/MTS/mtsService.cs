using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Utils;

namespace Services
{
    public class MtsService
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> UpdateDataAsync(string apiUrl, string mtsApiKey, string jsonFilePath)
        {
            var requestBody = FileUtils.ReadJsonFile(jsonFilePath);
            if (requestBody == null)
            {
                throw new ArgumentNullException(nameof(requestBody), "Request body cannot be null.");
            }

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("ApiKey", mtsApiKey);

            Console.WriteLine($"PUT Request to {apiUrl} with payload: {await content.ReadAsStringAsync()}");

            var response = await client.PutAsync(apiUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"PUT Request failed: {response.StatusCode}. Error: {responseContent}");
                throw new HttpRequestException($"Request to {apiUrl} failed: {response.StatusCode}");
            }

            return responseContent;
        }
    }
}
