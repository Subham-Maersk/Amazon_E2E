using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Utils;
using System;

namespace Services
{
    public class MtsService
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> UpdateDataAsync(string apiUrl, string mtsApiKey, string jsonFilePath)
        {
            try
            {
                var requestBody = FileUtils.ReadJsonFile(jsonFilePath);
                if (requestBody == null)
                {
                    throw new ArgumentNullException(nameof(requestBody), "Request body cannot be null.");
                }

                var requestBodyString = JsonConvert.SerializeObject(requestBody);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("ApiKey", mtsApiKey);

                var content = new StringContent(requestBodyString, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(apiUrl, content);

                Console.WriteLine($"PUT Request to {apiUrl} with payload: {requestBodyString}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine($"PUT Request succeeded. Response: {responseContent}");

                    return responseContent;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"PUT Request failed with status code: {response.StatusCode}. Error: {errorResponse}");

                    throw new HttpRequestException($"Request to {apiUrl} failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while calling MTS API: {ex.Message}");
                throw;
            }
        }
    }
}
