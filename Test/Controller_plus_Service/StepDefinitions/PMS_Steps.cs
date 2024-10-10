using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Services;
using Newtonsoft.Json.Linq;

namespace StepDefinitions
{
    [Binding]
    public class ShipmentServiceSteps
    {
        private string jwtToken;
        private string manifestNumber;
        private string customerIdentifier = "AMAEU0001";
        private JObject validationResponse;
        private JObject finalMileTrackingResponse;

        [Given(@"the user provides valid credentials")]
        public void GivenTheUserProvidesValidCredentials()
        {
            // This step automatically uses the credentials from the JSON file
            // You may want to add logic to assert that the credentials are being loaded correctly
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

        [When(@"the user validates the shipment creation with manifest number (.*)")]
        public async Task WhenTheUserValidatesTheShipmentCreation(string manifestNumber)
        {
            if (string.IsNullOrEmpty(jwtToken))
            {
                throw new Exception("JWT Token is null or empty before validation.");
            }

            this.manifestNumber = manifestNumber;
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

        [When(@"the user retrieves the final mile tracking for order (.*) with API key (.*)")]
        public async Task WhenTheUserRetrievesTheFinalMileTracking(string orderId, string apiKey)
        {
            finalMileTrackingResponse = await FinalMileService.GetFinalMileTracking(orderId, apiKey);
        }

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
