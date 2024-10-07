using NUnit.Framework;
using Services;
using Utils;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Tests
{
    [TestFixture]
    public class ShipmentTests
    {
        private string _jwtToken;
        private string _gtsJwtToken;
        private string _sortJwtToken; 
        private string _manifestNumber;
        private string _customerIdentifier = "AMAEU0001";
        private string _apiKey = "VkB3eHRnellwRWdBKmY0VlZ3aCpWSE5yQ2dwZTlaOUdqanhHNlBVd3BLeTdWaExweHcjOW8mWHpWRUFoSDNXMzk1cnBjNzVIQlJUaiZ1Q3F1Xk1QJE12dW4hcGlhY2I5aDNhY01WaFJWdDZhbkUlJHB1YXQmI2lDOV51V012SFU=";
        private string _mtsApiKey ="F2C03C5A-AAC8-44E1-AFE2-02EE5C7B62DB";
        private LabelGeneratorService _labelGeneratorService;
        private SortService _sortService;
        private MtsService _mtsService;
        private string _apiUrl = "https://mec-carrier-tracking-usps-api-dev.maersk-digital.dev/api/v4.0/Tracking"; // Update this with the actual MTS API URL

        [SetUp]
        public async Task Setup()
        {
            // Call UpdateShipmentDetails to increment manifest and update other fields
            string jsonFilePath = "Test_Access_Data_Layer/AMS_LAX_2709.json";
            FileUtils.UpdateShipmentDetails(jsonFilePath);

            // Read the updated manifest number from the JSON file
            var jsonContent = FileUtils.ReadJsonFile(jsonFilePath);
            _manifestNumber = jsonContent["lastManifestNumber"]?.ToString();

            // Retrieve JWT tokens
            _jwtToken = await AuthService.GetJwtToken();
            Assert.IsNotNull(_jwtToken, "Failed to retrieve JWT token");

            _sortService = new SortService();
            var sortCredentials = FileUtils.GetCredentials("Test_Access_Data_Layer/sort_credentials.json");
            string sortEmail = sortCredentials["email"].ToString();
            string sortPassword = sortCredentials["password"].ToString();
            _sortJwtToken = await _sortService.GetSortingJwtToken(sortEmail, sortPassword);
            Assert.IsNotNull(_sortJwtToken, "Failed to retrieve Sorting JWT token");
            TestContext.WriteLine("Sort JWT Token: " + _sortJwtToken);

            _labelGeneratorService = new LabelGeneratorService();
            _mtsService = new MtsService(); 
        }

        [Test, Category("Amazon")]
        public async Task TestShipmentApiAndValidationFlow()
        {
            var response = await ShipmentService.CallShipmentApi(_jwtToken, _manifestNumber, _customerIdentifier, "Test_Access_Data_Layer/AMS_LAX_2709.json");
            Assert.IsNotNull(response, "Manifest Number is already used.");

            var validationResponse = await ShipmentService.ValidateManifestCreation(_jwtToken, _manifestNumber, _customerIdentifier);
            Assert.IsNotNull(validationResponse, "No response received from manifest validation API");

            while (validationResponse["results"]?[0]?["status"]?.ToString() == "PreProcessing")
            {
                TestContext.WriteLine("Shipment is in PreProcessing, waiting for 30 seconds...");
                await Task.Delay(30000);

                validationResponse = await ShipmentService.ValidateManifestCreation(_jwtToken, _manifestNumber, _customerIdentifier);
                Assert.IsNotNull(validationResponse, "No response received from validation API after retry.");
            }

            Assert.AreEqual("Processed", validationResponse["results"]?[0]?["status"]?.ToString(), "Shipment processing failed.");
            TestContext.WriteLine("Shipment processed successfully.");

            var jsonFilePath = "Test_Access_Data_Layer/AMS_LAX_2709.json";
            var jsonContent = FileUtils.ReadJsonFile(jsonFilePath);
            TestContext.WriteLine("JSON Content: " + jsonContent.ToString());

            string orderId = jsonContent["shipments"]?[0]?["orderNumber"]?.ToString();
            Assert.IsNotNull(orderId, "Failed to retrieve Order ID from the JSON file.");
            TestContext.WriteLine("Order ID: " + orderId);

            var finalMileResponse = await FinalMileService.GetFinalMileTracking(orderId, _apiKey);
            Assert.IsNotNull(finalMileResponse, "Failed to retrieve final mile tracking number.");
            TestContext.WriteLine("Final Mile Tracking Number: " + finalMileResponse["finalMileTracking"]);

            _gtsJwtToken = await GtsAuthService.GetJwtTokenFromGts("Test_Access_Data_Layer/gts_credentials.json");
            Assert.IsNotNull(_gtsJwtToken, "Failed to retrieve GTS JWT token.");

            string trackingNumber = FileUtils.GetTrackingNumber(jsonFilePath);
            Assert.IsNotNull(trackingNumber, "Failed to retrieve Tracking Number from the JSON file.");
            TestContext.WriteLine($"Tracking Number: {trackingNumber}");

            // var gtsTrackingService = new GtsTrackingService();
            // string packageBarcode = await gtsTrackingService.GetPackageBarcodeAsync(_gtsJwtToken, trackingNumber);
            // Assert.IsNotNull(packageBarcode, "Failed to retrieve PackageBarcode from GTS tracking API.");
            // TestContext.WriteLine("PackageBarcode: " + packageBarcode);

            var labelResponse = await _labelGeneratorService.GenerateLabelAsync("Test_Access_Data_Layer/labelRequest.json", _jwtToken);
            Assert.IsNotNull(labelResponse, "Failed to generate label.");
            TestContext.WriteLine("Label Generation Response: " + labelResponse.ToString());

            string extractedId = ExtractIdFromLabelResponse(labelResponse);
            Assert.IsNotNull(extractedId, "No ID starting with 'amslax' found in the label response.");
            TestContext.WriteLine("Extracted ID: " + extractedId);

            // var assignResponse = await SortService.AssignContainerToParcels(extractedId, packageBarcode, _sortJwtToken);
            // Assert.IsNotNull(assignResponse, "Failed to assign container to parcels.");
            // TestContext.WriteLine("Container Assignment Response: " + assignResponse);

            var mtsResponse = await MtsService.UpdateDataAsync(_apiUrl, _mtsApiKey, "Test_Access_Data_Layer/mts_credentials.json");
            Assert.IsNotNull(mtsResponse, "Failed to call MTS service.");
            TestContext.WriteLine("MTS Service Response: " + mtsResponse);
        }

        private string ExtractIdFromLabelResponse(JToken labelResponse)
        {
            if (labelResponse is JArray labelArray)
            {
                foreach (var item in labelArray)
                {
                    string id = item["id"]?.ToString();
                    if (!string.IsNullOrEmpty(id) && id.StartsWith("amslax", StringComparison.OrdinalIgnoreCase))
                    {
                        return id;
                    }
                }
            }
            return null;
        }
    }
}
