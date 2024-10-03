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
        private string _manifestNumber = "Testing_20240_91";
        private string _customerIdentifier = "AMAEU0001";
        private string _apiKey = "VkB3eHRnellwRWdBKmY0VlZ3aCpWSE5yQ2dwZTlaOUdqanhHNlBVd3BLeTdWaExweHcjOW8mWHpWRUFoSDNXMzk1cnBjNzVIQlJUaiZ1Q3F1Xk1QJE12dW4hcGlhY2I5aDNhY01WaFJWdDZhbkUlJHB1YXQmI2lDOV51V012SFU=";
        private LabelGeneratorService _labelGeneratorService;
        private SortService _sortService;

        [SetUp]
        public async Task Setup()
        {
            var credentials = FileUtils.GetCredentials("Test_data/credentials.json");
            string email = credentials["email"].ToString();
            string password = credentials["password"].ToString();

            // Retrieve main JWT token
            _jwtToken = await AuthService.GetJwtToken(email, password);
            Assert.IsNotNull(_jwtToken, "Failed to retrieve JWT token");

            // Initialize SortService and retrieve sorting JWT token
            _sortService = new SortService();
            var sortCredentials = FileUtils.GetCredentials("Test_data/sort_credentials.json");
            string sortEmail = sortCredentials["email"].ToString();
            string sortPassword = sortCredentials["password"].ToString();
            _sortJwtToken = await _sortService.GetSortingJwtToken(sortEmail, sortPassword);
            Assert.IsNotNull(_sortJwtToken, "Failed to retrieve Sorting JWT token");
            TestContext.WriteLine("Sort JWT Token: " + _sortJwtToken);


            _labelGeneratorService = new LabelGeneratorService();
        }

        [Test]
        public async Task TestShipmentApiAndValidationFlow()
        {
            var response = await ShipmentService.CallShipmentApi(_jwtToken, _manifestNumber, _customerIdentifier, "Test_data/AMS_LAX_2709.json");
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

            var jsonFilePath = "Test_data/AMS_LAX_2709.json";
            var jsonContent = FileUtils.ReadJsonFile(jsonFilePath);
            TestContext.WriteLine("JSON Content: " + jsonContent.ToString());

            string orderId = jsonContent["shipments"]?[0]?["orderNumber"]?.ToString();
            Assert.IsNotNull(orderId, "Failed to retrieve Order ID from the JSON file.");
            TestContext.WriteLine("Order ID: " + orderId);

            var finalMileResponse = await FinalMileService.GetFinalMileTracking(orderId, _apiKey);
            Assert.IsNotNull(finalMileResponse, "Failed to retrieve final mile tracking number.");
            TestContext.WriteLine("Final Mile Tracking Number: " + finalMileResponse["finalMileTracking"]);

            _gtsJwtToken = await GtsAuthService.GetJwtTokenFromGts("Test_data/gts_credentials.json");
            Assert.IsNotNull(_gtsJwtToken, "Failed to retrieve GTS JWT token.");

            string trackingNumber = FileUtils.GetTrackingNumber(jsonFilePath);
            Assert.IsNotNull(trackingNumber, "Failed to retrieve Tracking Number from the JSON file.");
            TestContext.WriteLine($"Tracking Number: {trackingNumber}");

            var gtsTrackingService = new GtsTrackingService();
            string packageBarcode = await gtsTrackingService.GetPackageBarcodeAsync(_gtsJwtToken, trackingNumber);
            Assert.IsNotNull(packageBarcode, "Failed to retrieve PackageBarcode from GTS tracking API.");
            TestContext.WriteLine("PackageBarcode: " + packageBarcode);

            var labelResponse = await _labelGeneratorService.GenerateLabelAsync("Test_data/labelRequest.json", _jwtToken);
            Assert.IsNotNull(labelResponse, "Failed to generate label.");
            TestContext.WriteLine("Label Generation Response: " + labelResponse.ToString());

            string extractedId = ExtractIdFromLabelResponse(labelResponse);
            Assert.IsNotNull(extractedId, "No ID starting with 'amslax' found in the label response.");
            TestContext.WriteLine("Extracted ID: " + extractedId);

            var assignResponse = await SortService.AssignContainerToParcels(extractedId, packageBarcode, _sortJwtToken);
            Assert.IsNotNull(assignResponse, "Failed to assign container to parcels.");
            TestContext.WriteLine("Container Assignment Response: " + assignResponse);
        }

        private string ExtractIdFromLabelResponse(JToken labelResponse)
        {
            if (labelResponse is JArray labelArray)
            {
                foreach (var item in labelArray)
                {
                    var url = item.ToString();
                    if (url.Contains("amslax"))
                    {
                        var startIndex = url.IndexOf("amslax");
                        var endIndex = url.IndexOf('.', startIndex); 
                        if (endIndex == -1) endIndex = url.Length; 
                        return url.Substring(startIndex, endIndex - startIndex);
                    }
                }
            }
            return null; 
        }
    }
}
