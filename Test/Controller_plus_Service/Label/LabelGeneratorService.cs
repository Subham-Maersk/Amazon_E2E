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
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<JToken> GenerateLabelAsync(string labelRequestPath, string jwtToken)
        {
            var requestBody = await ReadJsonFileAsync(labelRequestPath) 
                               ?? throw new Exception("Failed to read request body from JSON file.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var response = await _httpClient.PostAsync(EnvironmentConfig.GetAllUrls()["MLO_E2E"], 
                new StringContent(requestBody.ToString(), System.Text.Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode
                ? JToken.Parse(await response.Content.ReadAsStringAsync())
                : throw new Exception($"Error generating label: {response.ReasonPhrase}");
        }

        private async Task<JToken> ReadJsonFileAsync(string filePath) =>
            JToken.Parse(await File.ReadAllTextAsync(filePath));
    }
}
