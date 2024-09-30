using NUnit.Framework;
using Services;
using Utils;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class ShipmentTests
    {
        private string _jwtToken;
        private string _manifestNumber = "Testing_20240_32"; 
        private string _customerIdentifier = "AMAEU0001"; 

        [SetUp]
        public async Task Setup()
        {
            var credentials = FileUtils.GetCredentials("Test_data/credentials.json");
            string email = credentials["email"].ToString();
            string password = credentials["password"].ToString();

            _jwtToken = await AuthService.GetJwtToken(email, password);
            Assert.IsNotNull(_jwtToken, "Failed to retrieve JWT token");
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
        }
    }
}
