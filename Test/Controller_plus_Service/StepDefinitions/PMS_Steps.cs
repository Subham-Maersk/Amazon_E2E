using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Services;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Utils;

namespace StepDefinitions
{
    [Binding]
    [Category("Amazon")]
    public class AuthServiceSteps
    {
        private string jwtToken;
        private JObject validationResponse; 
        private JObject finalMileTrackingResponse; 

        [Given(@"the user login for manifest creation")]
        public void GivenTheUserLoginForManifestCreation()
        {
        }

        [When(@"the user validates the shipment creation")]
        public async Task WhenTheUserValidatesTheShipmentCreation()
        {
            jwtToken = await AuthService.GetJwtToken();

            string jsonFilePath = "Test_Access_Data_Layer/AMS_LAX_2709.json"; 
            FileUtils.UpdateShipmentDetails(jsonFilePath);
            var jsonContent = FileUtils.ReadJsonFile(jsonFilePath); 

            string manifestNumber = jsonContent["lastManifestNumber"]?.ToString();
            string customerIdentifier = "AMAEU0001"; 

            validationResponse = await ShipmentService.ValidateManifestCreation(jwtToken, manifestNumber, customerIdentifier);
        }

        [Then(@"the user should receive a validation response")]
        public void ThenTheUserShouldReceiveAValidationResponse()
        {
            if (validationResponse == null)
            {
                throw new Exception("Validation response was not received.");
            }
            TestContext.WriteLine("Validation Response: " + validationResponse.ToString());
        }

        [When(@"the user retrieves the final mile tracking")]
        public async Task WhenTheUserRetrievesTheFinalMileTracking()
        {
            string orderId = "YourOrderId"; 
            string apiKey = "VkB3eHRnellwRWdBKmY0VlZ3aCpWSE5yQ2dwZTlaOUdqanhHNlBVd3BLeTdWaExweHcjOW8mWHpWRUFoSDNXMzk1cnBjNzVIQlJUaiZ1Q3F1Xk1QJE12dW4hcGlhY2I5aDNhY01WaFJWdDZhbkUlJHB1YXQmI2lDOV51V012SFU="; // Use actual API key

            finalMileTrackingResponse = await FinalMileService.GetFinalMileTracking(orderId, apiKey);
        }

        [Then(@"the user should receive the final mile tracking response")]
        public void ThenTheUserShouldReceiveTheFinalMileTrackingResponse()
        {
            if (finalMileTrackingResponse == null)
            {
                throw new Exception("Final mile tracking response was not received.");
            }
            TestContext.WriteLine("Final Mile Tracking Response: " + finalMileTrackingResponse.ToString());
        }
    }
}
