using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Services;
using Utils;
using System.Threading.Tasks;

namespace StepDefinitions
{
    [Binding]
    public class SortServiceSteps
    {
        private SortService _sortService;
        private string _sortJwtToken;
        private string _responseContent;

        [Given(@"I have valid sorting credentials")]
        public void GivenIHaveValidSortingCredentials()
        {
            // Credentials are stored in the JSON file and will be retrieved in the next step.
        }

        [When(@"I retrieve the sorting JWT token with valid credentials")]
        public async Task WhenIRetrieveTheSortingJwtTokenWithValidCredentials()
        {
            _sortService = new SortService();
            var sortCredentials = FileUtils.GetCredentials("Test_Access_Data_Layer/sort_credentials.json");
            string sortEmail = sortCredentials["email"].ToString();
            string sortPassword = sortCredentials["password"].ToString();
            _sortJwtToken = await _sortService.GetSortingJwtToken(sortEmail, sortPassword);
        }

        [Then(@"I should receive a valid JWT token")]
        public void ThenIShouldReceiveAValidJwtToken()
        {
            Assert.IsNotNull(_sortJwtToken, "Failed to retrieve Sorting JWT token.");
            TestContext.WriteLine("Sort JWT Token: " + _sortJwtToken);
        }

        [Given(@"I have a valid sorting JWT token")]
        public void GivenIHaveAValidSortingJwtToken()
        {
            Assert.IsNotNull(_sortJwtToken, "Sorting JWT token is not set.");
        }

        [When(@"I assign the container ""(.*)"" to parcel with barcode ""(.*)""")]
        public async Task WhenIAssignTheContainerToParcelWithBarcode(string containerId, string packageBarcode)
        {
            _responseContent = await SortService.AssignContainerToParcels(containerId, packageBarcode, _sortJwtToken);
        }

        [Then(@"I should receive a successful assignment response")]
        public void ThenIShouldReceiveASuccessfulAssignmentResponse()
        {
            Assert.IsNotNull(_responseContent, "Failed to assign container to parcels.");
            TestContext.WriteLine("Assign Container Response: " + _responseContent);
        }
    }
}
