using System.Threading.Tasks;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Services;
using Newtonsoft.Json.Linq;


[Binding]
public class GtsAuthSteps
{
    private string _gtsJwtToken;
    private string _trackingNumber;
    private string _packageBarcode;

    [Given(@"I have valid credentials for GTS login")]
    public void GivenIHaveValidCredentialsForGTSLogin()
    {
        // credentials are stored in the JSON file.
    }

    [When(@"I request a JWT token from GTS")]
    public async Task WhenIRequestAJWTTokenFromGTS()
    {
        _gtsJwtToken = await GtsAuthService.GetJwtTokenFromGts("path/to/credentials.json");
        Assert.IsNotNull(_gtsJwtToken, "JWT token retrieval failed.");
    }

    [Then(@"I should receive a valid JWT token")]
    public void ThenIShouldReceiveAValidJwtToken()
    {
        Assert.IsNotNull(_gtsJwtToken, "JWT token is null.");
        Console.WriteLine("Valid GTS JWT token received: " + _gtsJwtToken);
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
        var trackingService = new GtsTrackingService();
        _packageBarcode = await trackingService.GetPackageBarcodeAsync(_gtsJwtToken, _trackingNumber);
        Assert.IsNotNull(_packageBarcode, "Failed to retrieve package barcode.");
    }

    [Then(@"the package barcode should be returned successfully")]
    public void ThenThePackageBarcodeShouldBeReturnedSuccessfully()
    {
        Assert.IsNotNull(_packageBarcode, "Package barcode is null.");
        Console.WriteLine("Package Barcode: " + _packageBarcode);
    }
}
