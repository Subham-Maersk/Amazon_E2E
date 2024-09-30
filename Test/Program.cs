// using System;
// using System.IO;
// using System.Threading.Tasks;
// using Services;
// using Utils;

// class Program
// {
//     static async Task Main(string[] args)
//     {
//         var credentials = FileUtils.GetCredentials("Test_data/credentials.json");
//         string email = credentials["email"].ToString();
//         string password = credentials["password"].ToString();

//         string jwtToken = await AuthService.GetJwtToken(email, password);

//         if (!string.IsNullOrEmpty(jwtToken))
//         {
//             string manifestNumber = "Testing_20240_25";
//             string customerIdentifier = "AMAEU0001";

//             var response = await ShipmentService.CallShipmentApi(jwtToken, manifestNumber, customerIdentifier, "Test_data/AMS_LAX_2709.json");

//             if (response != null)
//             {
//                 Console.WriteLine("Response received from Shipment API:");
//                 Console.WriteLine(response.ToString());
//             }
//             else
//             {
//                 Console.WriteLine("No response received from Shipment API.");
//                 return;
//             }

//             var validationResponse = await ShipmentService.ValidateManifestCreation(jwtToken, manifestNumber, customerIdentifier);

//             while (validationResponse != null && validationResponse["results"]?[0]?["status"]?.ToString() == "PreProcessing")
//             {
//                 Console.WriteLine("Shipment is in PreProcessing, waiting for 30 seconds...");
//                 await Task.Delay(30000);

//                 validationResponse = await ShipmentService.ValidateManifestCreation(jwtToken, manifestNumber, customerIdentifier);

//                 if (validationResponse != null)
//                 {
//                     Console.WriteLine("Validation Response after retry:");
//                     Console.WriteLine(validationResponse.ToString());
//                 }
//                 else
//                 {
//                     Console.WriteLine("No response received from validation API after retry.");
//                     return;
//                 }
//             }

//             if (validationResponse != null && validationResponse["results"]?[0]?["status"]?.ToString() == "Processed")
//             {
//                 Console.WriteLine("Shipment processed successfully.");
//             }
//             else
//             {
//                 Console.WriteLine("Shipment processing failed or response is null.");
//                 if (validationResponse != null)
//                 {
//                     Console.WriteLine("Error Response: " + validationResponse.ToString());
//                 }
//             }
//         }
//         else
//         {
//             Console.WriteLine("Failed to retrieve JWT token.");
//         }
//     }
// }
