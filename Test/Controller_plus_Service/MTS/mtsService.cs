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

        // PUT method to update data by sending JSON body from a file
        public static async Task<string> UpdateDataAsync(string apiUrl, string mtsApiKey, string jsonFilePath)
        {
            try
            {
                // Read JSON body from the file located in Test_data folder
                var requestBody = FileUtils.ReadJsonFile(jsonFilePath);
                if (requestBody == null)
                {
                    throw new ArgumentNullException(nameof(requestBody), "Request body cannot be null.");
                }

                // Convert the request body into a JSON string
                var requestBodyString = JsonConvert.SerializeObject(requestBody);

                // Set the API key in the request headers
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("ApiKey", mtsApiKey);

                // Create HTTP content for the PUT request
                var content = new StringContent(requestBodyString, Encoding.UTF8, "application/json");

                // Send the PUT request
                var response = await client.PutAsync(apiUrl, content);

                // Log the request and response for debugging purposes
                Console.WriteLine($"PUT Request to {apiUrl} with payload: {requestBodyString}");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"PUT Request succeeded. Response: {responseContent}");

                    return responseContent;
                }
                else
                {
                    // Log the error and throw exception for failed requests
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"PUT Request failed with status code: {response.StatusCode}. Error: {errorResponse}");

                    throw new HttpRequestException($"Request to {apiUrl} failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow it
                Console.WriteLine($"An error occurred while calling MTS API: {ex.Message}");
                throw;
            }
        }
    }
}
