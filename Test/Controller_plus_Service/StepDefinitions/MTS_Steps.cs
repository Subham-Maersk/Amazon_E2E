using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Services;
using Utils;
using Configuration;
using Newtonsoft.Json.Linq; 
using System.Threading.Tasks;

namespace Steps
{
    [Binding]
    [Category("Amazon")]
    public class MtsServiceSteps
    {
        private string _apiUrl;
        private string _mtsApiKey;
        private string _jsonFilePath;
        private string _responseContent;

        [Given(@"User login usps api")]
        public void GivenUserLoginUspsApi()
        {
            TestContext.WriteLine("User logged into USPS API");
            
            _apiUrl = EnvironmentConfig.GetAllUrls().GetValueOrDefault("MTS_E2E");
            
            _mtsApiKey = "F2C03C5A-AAC8-44E1-AFE2-02EE5C7B62DB"; 
            
            _jsonFilePath = "Test_Access_Data_Layer/mts_credentials.json";
        }

        [When(@"User update data using the MTS API")]
        public async Task WhenUserUpdateDataUsingTheMtsApi()
        {
            _responseContent = await MtsService.UpdateDataAsync(_apiUrl, _mtsApiKey, _jsonFilePath);
            TestContext.WriteLine($"Response Content: {_responseContent}"); 
        }

        [Then(@"User should receive a success response")]
        public void ThenUserShouldReceiveASuccessResponse()
        {
            Assert.IsNotNull(_responseContent, "Response content is null.");

            var jsonResponse = JObject.Parse(_responseContent);
            var status = jsonResponse["trackings"]?[0]?["shipmentTrackings"]?[0]?["status"]?.ToString(); // Extract the status

            Assert.AreEqual("Delivered", status, $"Expected status to be 'Delivered', but was '{status}'.");

            if (status == "Delivered")
            {
                TestContext.WriteLine("Successful");
            }
        }
    }
}
