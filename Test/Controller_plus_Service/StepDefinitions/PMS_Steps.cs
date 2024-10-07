using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Services;
using Newtonsoft.Json.Linq;

namespace StepDefinitions
{
    [Binding]
    public class AuthServiceSteps
    {
        private string jwtToken;
        private JObject validationResponse; 
        private JObject finalMileTrackingResponse;

        [Given(@"the user provides valid credentials")]
        public void GivenTheUserProvidesValidCredentials()
        {
            // This step automatically uses the credentials from the JSON file
        }

        [When(@"the user submits the login request")]
        public async Task WhenTheUserSubmitsTheLoginRequest()
        {
            jwtToken = await AuthService.GetJwtToken();
        }

        [Then(@"the user should receive a JWT token")]
        public void ThenTheUserShouldReceiveAJwtToken()
        {
            if (string.IsNullOrEmpty(jwtToken))
            {
                throw new Exception("JWT token was not received.");
            }
            Console.WriteLine("JWT Token: " + jwtToken);
        }

        [When(@"the user validates the shipment creation")]
        public async Task WhenTheUserValidatesTheShipmentCreation()
        {
            string manifestNumber = "Testing_20240_123"; 
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
            Console.WriteLine("Validation Response: " + validationResponse.ToString());
        }

        // [When(@"the user retrieves the final mile tracking")]
        // public async Task WhenTheUserRetrievesTheFinalMileTracking()
        // {
        //     // Example order ID
        //     string orderId = "YourOrderId"; // Update with the actual order ID
        //     string apiKey = "YourApiKey"; // Update with the actual API key

        //     finalMileTrackingResponse = await FinalMileService.GetFinalMileTracking(orderId, apiKey);
        // }

        [Then(@"the user should receive the final mile tracking response")]
        public void ThenTheUserShouldReceiveTheFinalMileTrackingResponse()
        {
            if (finalMileTrackingResponse == null)
            {
                throw new Exception("Final mile tracking response was not received.");
            }
            Console.WriteLine("Final Mile Tracking Response: " + finalMileTrackingResponse.ToString());
        }
    }
}
