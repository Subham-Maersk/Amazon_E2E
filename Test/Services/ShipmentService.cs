using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Services
{
    public static class ShipmentService
    {
        private static readonly string shipmentUrl = "https://as-indicina-ncus-sandbox.azurewebsites.net/api/v1/Shipment/Manifest";

        public static async Task<JObject> CallShipmentApi(string jwtToken, string manifestNumber, string customerIdentifier, string filePath)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                Console.WriteLine("Manifest Number: " + manifestNumber);
                Console.WriteLine("Customer Identifier: " + customerIdentifier);

                var shipmentDataJson = File.ReadAllText(filePath);
                
                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    var numberContent = new StringContent(manifestNumber); 
                    var customerContent = new StringContent(customerIdentifier);
                    
                    multipartFormContent.Add(numberContent, "number"); 
                    multipartFormContent.Add(customerContent, "customerIdentifier"); 

                    var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(shipmentDataJson));
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    multipartFormContent.Add(fileContent, "file", Path.GetFileName(filePath));

                    HttpResponseMessage response = await client.PostAsync(shipmentUrl, multipartFormContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        return JObject.Parse(responseBody);
                    }
                    else
                    {
                        var errorResponseBody = await response.Content.ReadAsStringAsync();
                        return JObject.Parse(errorResponseBody);
                    }
                }
            }
        }

        public static async Task<JObject> ValidateManifestCreation(string jwtToken, string manifestNumber, string customerIdentifier)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                string validationUrl = $"https://as-indicina-ncus-sandbox.azurewebsites.net/api/v1/shipment/manifest?customerIdentifier={customerIdentifier}&number={manifestNumber}";

                HttpResponseMessage response = await client.GetAsync(validationUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(responseBody);
                }
                else
                {
                    var errorResponseBody = await response.Content.ReadAsStringAsync();
                    return null;
                }
            }
        }
    }
}
