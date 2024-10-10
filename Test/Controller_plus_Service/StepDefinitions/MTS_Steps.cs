using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Services;
using Utils;
using System.Threading.Tasks;

namespace Steps
{
    [Binding]
    public class MtsServiceSteps
    {
        private string _apiUrl;
        private string _mtsApiKey;
        private string _jsonFilePath;
        private string _responseContent;

        [Given(@"I have a valid MTS API URL")]
        public void GivenIHaveAValidMtsApiUrl()
        {
            _apiUrl = "https://mec-carrier-tracking-usps-api-dev.maersk-digital.dev/api/v4.0/Tracking"; 
        }

        [Given(@"I have a valid API key")]
        public void GivenIHaveAValidApiKey()
        {
            _mtsApiKey = "F2C03C5A-AAC8-44E1-AFE2-02EE5C7B62DB"; 
        }

        [Given(@"I have a valid JSON file path for the request")]
        public void GivenIHaveAValidJsonFilePathForTheRequest()
        {
            _jsonFilePath = "Test_Access_Data_Layer/mts_credentials.json"; // Replace with your actual JSON file path
        }

        [When(@"I update data using the MTS API")]
        public async Task WhenIUpdateDataUsingTheMtsApi()
        {
            _responseContent = await MtsService.UpdateDataAsync(_apiUrl, _mtsApiKey, _jsonFilePath);
        }

        [Then(@"I should receive a success response")]
        public void ThenIShouldReceiveASuccessResponse()
        {
            
            Assert.IsNotNull(_responseContent);
            Assert.IsTrue(_responseContent.Contains("Success")); 
        }
    }
}
