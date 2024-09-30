using NUnit.Framework;
using Services;
using Utils;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class ShipmentTests
    {
        private string jwtToken;
        private string manifestNumber = "Testing_20240_32", customerIdentifier = "AMAEU0001"; 

        [SetUp]
        public async Task Setup()
        {
            var credentials = FileUtils.GetCredentials("Test_data/credentials.json");
            string email = credentials["email"].ToString();
            string password = credentials["password"].ToString();

            jwtToken = await AuthService.GetJwtToken(email, password);
            Assert.IsNotNull(jwtToken, "Failed to retrieve JWT token");
        }

        [Test]
        public async Task TestShipmentApiAndValidationFlow()
        {
            var response = await ShipmentService.CallShipmentApi(jwtToken, manifestNumber, customerIdentifier, "Test_data/AMS_LAX_2709.json");

            Assert.IsNotNull(response, "Manifest Number is already used.");

            var validationResponse = await ShipmentService.ValidateManifestCreation(jwtToken, manifestNumber, customerIdentifier);
            Assert.IsNotNull(validationResponse, "No response received from manifest validation API");

            while (validationResponse["results"]?[0]?["status"]?.ToString() == "PreProcessing")
            {
                await Task.Delay(30000); 

                validationResponse = await ShipmentService.ValidateManifestCreation(jwtToken, manifestNumber, customerIdentifier);
                Assert.IsNotNull(validationResponse, "No response received from validation API after retry.");
            }

            Assert.AreEqual("Processed", validationResponse["results"]?[0]?["status"]?.ToString(), "Shipment processing failed.");
            TestContext.WriteLine("Shipment processed successfully.");
        }
    }
}
