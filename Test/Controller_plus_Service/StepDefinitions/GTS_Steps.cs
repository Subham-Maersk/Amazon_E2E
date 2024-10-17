using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Utils;
using Configuration;
using Allure.Commons;
using Allure.NUnit.Attributes; 
[Binding]
[Category("Amazon")]
[AllureSuite("GTS")]
public class GtsAuthSteps
{
    private string _gtsJwtToken;
    private string _trackingNumber;
    private string _packageBarcode;
    private static readonly string loginUrl = "https://api-oct.maersk.com/api/v1/Login";
    private static readonly HttpClient _httpClient = new HttpClient(); 

    [Given(@"I have valid credentials for GTS login")]
    public void GivenIHaveValidCredentialsForGTSLogin()
    {
    }

    [When(@"I request a JWT token from GTS")]
    public async Task WhenIRequestAJWTTokenFromGTS()
    {
        var credentials = FileUtils.GetCredentials("Test_Access_Data_Layer/gts_credentials.json");
        var loginData = new
        {
            email = credentials["email"].ToString(),
            password = credentials["password"].ToString()
        };

        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(loginUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
                _gtsJwtToken = jsonResponse["data"]?["jwt"]?.ToString();
            }
        }
        catch (HttpRequestException)
        {
            Assert.Fail("HTTP request failed.");
        }
        catch (Exception)
        {
            Assert.Fail("An error occurred while requesting JWT token.");
        }

        Assert.IsNotNull(_gtsJwtToken, "JWT token retrieval failed.");
    }

    [Then(@"I should receive a valid GTS JWT token")]
    public void ThenIShouldReceiveAValidJwtToken()
    {
        Assert.IsNotNull(_gtsJwtToken, "JWT token is null.");
        TestContext.WriteLine("Valid GTS JWT token received: " + _gtsJwtToken);
    }

    [Given(@"I have a valid GTS JWT token")]
    public void GivenIHaveAValidGTSJwtToken()
    {
        Assert.IsNotNull(_gtsJwtToken, "JWT token is required.");
    }

    [Given(@"I have a shipment tracking number ""(.*)""")]
    public void GivenIHaveAShipmentTrackingNumber(string trackingNumber)
    {
        _trackingNumber = trackingNumber;
    }

    [When(@"I request the package barcode from GTS tracking service")]
    public async Task WhenIRequestThePackageBarcodeFromGTSTrackingService()
    {
        EnvironmentConfig.GetEnvironment("GTS_E2E");
        string apiUrl = $"{EnvironmentConfig.BaseURL}?TrackingNumber={_trackingNumber}";
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _gtsJwtToken);

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var jsonResponse = JToken.Parse(await response.Content.ReadAsStringAsync());
            _packageBarcode = jsonResponse.Type switch
            {
                JTokenType.Array => jsonResponse[0]["PackageBarcode"]?.ToString(),
                JTokenType.Object => jsonResponse["PackageBarcode"]?.ToString(),
                _ => throw new Exception("Unexpected JSON format.")
            };
        }
        catch (HttpRequestException)
        {
            Assert.Fail("HTTP request failed.");
        }
        catch (Exception)
        {
            Assert.Fail("An error occurred while requesting the package barcode.");
        }

        Assert.IsNotNull(_packageBarcode, "Failed to retrieve package barcode.");
    }

    [Then(@"the package barcode should be returned successfully")]
    public void ThenThePackageBarcodeShouldBeReturnedSuccessfully()
    {
        Assert.IsNotNull(_packageBarcode, "Package barcode is null.");
        TestContext.WriteLine("Package Barcode: " + _packageBarcode);
    }

    [Then(@"I log the GTS token and package barcode")]
    public void ThenILogTheGTSTokenAndPackageBarcode()
    {
        TestContext.WriteLine("GTS JWT Token: " + _gtsJwtToken);
        TestContext.WriteLine("Package Barcode: " + _packageBarcode);
    }
}
