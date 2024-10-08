using System.Threading.Tasks;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Services;
using Newtonsoft.Json.Linq;

[Binding]
public class LabelGeneratorServiceSteps
{
    private LabelGeneratorService _labelGeneratorService;
    private string _jwtToken;
    private JToken _labelResponse;

    public LabelGeneratorServiceSteps()
    {
        _labelGeneratorService = new LabelGeneratorService();
    }

    [Given(@"I have a valid JWT token for shipment service")]
    public async Task GivenIHaveAValidJwtTokenForShipmentService()
    {
        _jwtToken = await AuthService.GetJwtToken();
        Assert.IsNotNull(_jwtToken, "Failed to retrieve JWT token.");
    }

    [Given(@"I have a label request JSON file at ""(.*)""")]
    public void GivenIHaveALabelRequestJsonFileAt(string filePath)
    {
        Assert.IsTrue(File.Exists(filePath), "Label request JSON file not found.");
    }

    [When(@"I generate a label using the label generator service")]
    public async Task WhenIGenerateALabelUsingTheLabelGeneratorService()
    {
        _labelResponse = await _labelGeneratorService.GenerateLabelAsync("Test_Access_Data_Layer/labelRequest.json", _jwtToken);
        Assert.IsNotNull(_labelResponse, "Label generation failed.");
    }

    [Then(@"the label should be generated successfully")]
    public void ThenTheLabelShouldBeGeneratedSuccessfully()
    {
        Assert.IsNotNull(_labelResponse, "Label response is null.");
        TestContext.WriteLine("Label Generation Response: " + _labelResponse.ToString());
    }

    [Then(@"the label response should contain the expected 'ams' ID")]
    public void ThenTheLabelResponseShouldContainTheExpectedAmslaxId()
    {
        string extractedId = ExtractIdFromLabelResponse(_labelResponse);
        Assert.IsNotNull(extractedId, "No ID starting with 'ams' found in the label response.");
        TestContext.WriteLine("Extracted ID: " + extractedId);
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
