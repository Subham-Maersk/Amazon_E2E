using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Configuration; 
namespace Services
{
    public class LabelGeneratorService
    {
        private readonly HttpClient _httpClient;

        public LabelGeneratorService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<JToken> GenerateLabelAsync(string labelRequestPath, string jwtToken)
        {
            var requestBody = await ReadJsonFileAsync(labelRequestPath);
            if (requestBody == null)
            {
                throw new Exception("Failed to read request body from JSON file.");
            }

            string apiUrl = EnvironmentConfig.GetAllUrls()["MLO_E2E"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var response = await _httpClient.PostAsync(apiUrl, new StringContent(requestBody.ToString(), System.Text.Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JToken.Parse(jsonResponse);
            }
            else
            {
                throw new Exception($"Error generating label: {response.ReasonPhrase}");
            }
        }

        private async Task<JToken> ReadJsonFileAsync(string filePath)
        {
            try
            {
                var jsonContent = await File.ReadAllTextAsync(filePath);
                return JToken.Parse(jsonContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading JSON file: {ex.Message}");
            }
        }
    }
}
