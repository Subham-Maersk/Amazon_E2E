using System.IO;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Services;
using Newtonsoft.Json.Linq;
using Allure.Commons;
using Allure.NUnit.Attributes;

[Binding]
[Category("Amazon")] 
[AllureSuite("GTS")]
[AllureSubSuite("Label_Generation")]
public class LabelGeneratorServiceSteps
{
    private LabelGeneratorService _labelGeneratorService;
    private string _jwtToken;
    private JToken _labelResponse;
    private string _containerId; 

    public LabelGeneratorServiceSteps()
    {
        _labelGeneratorService = new LabelGeneratorService();
    }

    [Given(@"I have a valid JWT token for shipment service")]
    [AllureTag("Amazon")]
    public async Task GivenIHaveAValidJwtTokenForShipmentService()
    {
        _jwtToken = await AuthService.GetJwtToken();
        Assert.IsNotNull(_jwtToken, "Failed to retrieve JWT token.");
    }

    [When(@"User hit label generation api")]
    [AllureTag("Amazon")]

    public async Task WhenUserHitLabelGenerationApi()
    {
        string filePath = "Test_Access_Data_Layer/labelRequest.json";
        Assert.IsTrue(File.Exists(filePath), "Label request JSON file not found.");

        _labelResponse = await _labelGeneratorService.GenerateLabelAsync(filePath, _jwtToken);
        Assert.IsNotNull(_labelResponse, "Label generation failed.");

        _containerId = ExtractIdFromLabelResponse(_labelResponse);
        Assert.IsNotNull(_containerId, "Extracted Container ID is null.");
    }

    [Then(@"the label response should contain the expected 'ams' ID")]
    [AllureTag("Amazon")]

    public void ThenTheLabelResponseShouldContainTheExpectedAmsId()
    {
        Assert.IsNotNull(_containerId, "No ID starting with 'ams' found in the label response.");
        TestContext.WriteLine("Extracted Container ID: " + _containerId);
    }

    private string ExtractIdFromLabelResponse(JToken labelResponse)
    {
        if (labelResponse is JArray labelArray)
        {
            foreach (var item in labelArray)
            {
                var url = item.ToString();
                if (url.Contains("ams"))
                {
                    var startIndex = url.IndexOf("ams");
                    var endIndex = url.IndexOf('.', startIndex);
                    if (endIndex == -1) endIndex = url.Length;
                    return url.Substring(startIndex, endIndex - startIndex);
                }
            }
        }
        return null; 
    }
}
