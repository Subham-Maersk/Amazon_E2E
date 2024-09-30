// using System;
// using System.IO;
// using System.Net.Http;
// using System.Net.Http.Headers;
// using System.Net.Http.Json;
// using System.Text.Json;
// using System.Threading.Tasks;

// class Program
// {
//     public class ManifestResponse
//     {
//         public string Status { get; set; }
//         public string Message { get; set; }
//     }

//     public class ManifestParams
//     {
//         public string CustomerIdentifier { get; set; }
//         public string ManifestNo { get; set; }
//         public string FinalMileNo { get; set; }
//     }

//     private static async Task<ManifestResponse> HitManifestApi(string jwtToken, ManifestParams manifestParams)
//     {
//         using (var httpClient = new HttpClient())
//         {
//             var manifestApiUrl = "https://as-indicina-ncus-sandbox.azurewebsites.net/api/v1/Shipment/Manifest";

//             httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

//             var response = await httpClient.PostAsJsonAsync(manifestApiUrl, manifestParams);

//             if (response.IsSuccessStatusCode)
//             {
//                 var manifestResponse = await response.Content.ReadFromJsonAsync<ManifestResponse>();
//                 return manifestResponse;
//             }
//             else
//             {
//                 throw new Exception($"Failed to call the API. Status code: {response.StatusCode}");
//             }
//         }
//     }

//     private static ManifestParams ReadManifestParamsFromFile(string filePath)
//     {
//         var jsonData = File.ReadAllText(filePath);
//         return JsonSerializer.Deserialize<ManifestParams>(jsonData);
//     }

//     static async Task Main(string[] args)
//     {
//         try
//         {
//             string baseFolder = "test_data";
//             string paramsFilePath = Path.Combine(baseFolder, "manifest_params.json");

//             // Fetch JWT token from the JwtService
//             string jwtToken = await JwtService.GetJwtTokenAsync();

//             // Read manifest parameters from file
//             ManifestParams manifestParams = ReadManifestParamsFromFile(paramsFilePath);

//             // Call the API with the JWT token and parameters
//             var manifestResponse = await HitManifestApi(jwtToken, manifestParams);
//             Console.WriteLine($"API Response: Status = {manifestResponse.Status}, Message = {manifestResponse.Message}");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Error: {ex.Message}");
//         }
//     }
// }
