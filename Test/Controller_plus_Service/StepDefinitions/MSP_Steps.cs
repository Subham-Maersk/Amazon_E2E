using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Services;
using Utils;
using System.Threading.Tasks;

namespace StepDefinitions
{
    [Binding]
    [Category("Amazon")]
    public class SortServiceSteps
    {
        private SortService _sortService;
        private string _sortJwtToken;
        private string _responseContent;

        [Given(@"user login for containerization")]
        public async Task GivenUserLoginForContainerization()
        {
            _sortService = new SortService();
            var sortCredentials = FileUtils.GetCredentials("Test_Access_Data_Layer/sort_credentials.json");
            string sortEmail = sortCredentials["email"].ToString();
            string sortPassword = sortCredentials["password"].ToString();
            _sortJwtToken = await _sortService.GetSortingJwtToken(sortEmail, sortPassword);

            Assert.IsNotNull(_sortJwtToken, "Failed to retrieve Sorting JWT token.");
            TestContext.WriteLine("Sort JWT Token: " + _sortJwtToken);
        }

        [When(@"assign the container to parcel with barcode ""(.*)"" and ""(.*)""")]
        public async Task WhenAssignTheContainerToParcelWithBarcode(string containerId, string packageBarcode)
        {
            Assert.IsNotNull(_sortJwtToken, "Sorting JWT token is not set.");
            _responseContent = await SortService.AssignContainerToParcels(containerId, packageBarcode, _sortJwtToken); // Call as static
        }

        [Then(@"User receive successful response")]
        public void ThenUserReceiveSuccessfulResponse()
        {
            Assert.IsNotNull(_responseContent, "Failed to assign container to parcels.");
            TestContext.WriteLine("Assign Container Response: " + _responseContent);
        }
    }
}
