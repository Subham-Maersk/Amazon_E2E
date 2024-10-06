using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Amazon_E2E_copy.Tests
{
    public class LaunchGoogleTest
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false 
            });
        }

        [Test, Category("Custom")] 
        public async Task LaunchGoogleTestAsync()
        {
            _page = await _browser.NewPageAsync();
           await _page.GotoAsync("https://internalportal-test.b2ceurope.eu/datatable/international_customs"); 
           // Assert.AreEqual("Google", await _page.TitleAsync()); 

           await _page.FillAsync("//input[@id='username']", "adarsh.k@maersk.com"); 
      await _page.FillAsync("//input[@id='password']", "ho6!NX4x2edb%S8CeJgW2t"); 
      await _page.ClickAsync("//button[@type='submit']");
      
      await Task.Delay(1200);
      await _page.ClickAsync("//a[text()='Customs Tools']");
      await _page.ClickAsync("(//a[text()='International Customs'])[1]");
      await _page.ClickAsync("//button[@class='dt-button buttons-create']");
      await _page.ClickAsync("(//button[@class='editor-datetime-button editor-datetime-day'])[7]");
      await _page.ClickAsync("//input[@id='DTE_Field_InternationalCustoms-ArrivalDate']");
      await _page.ClickAsync("(//button[@class='editor-datetime-button editor-datetime-day'])[7]");
     var dropdownSelector = "//select[@id='DTE_Field_InternationalCustoms-LKAirportOriginID']"; // This is the ID or selector of the <select> element

        await _page.Locator(dropdownSelector).SelectOptionAsync(new[] { "AMS (Netherlands)" });

        var selectedValue = await _page.Locator(dropdownSelector).InputValueAsync();
        Console.WriteLine($"Selected value: {selectedValue}");
        await Task.Delay(1200);
      
         var dropdownSelector2 = "//select[@id='DTE_Field_InternationalCustoms-LKAirportDestinationID']"; // This is the ID or selector of the <select> element
        await _page.Locator(dropdownSelector2).SelectOptionAsync(new[] { "JFK (United States)" });
        var selectedValue2 = await _page.Locator(dropdownSelector2).InputValueAsync();
        Console.WriteLine($"Selected value2: {selectedValue2}");
         await Task.Delay(1200);
            await _page.ClickAsync("//button[text()='Create']");
    
            await _page.ClickAsync("//ul[@class='dropbtn icons btn-right showLeft']");
    
         var dropdownSelector3 = "//div[@id='myDropdown3339']"; // This is the ID or selector of the <select> element

        await _page.Locator(dropdownSelector3).SelectOptionAsync(new[] { "Insert Overpacks" });

        var selectedValue3 = await _page.Locator(dropdownSelector).InputValueAsync();
        Console.WriteLine($"Selected value: {selectedValue3}");
        await Task.Delay(1200);
await _page.FillAsync("//textarea[@id='dispatch_numbers']","AMSLAXWSSF815XUYXW000001088");
await _page.ClickAsync("//button[@id='send-dispatches']");
await _page.ClickAsync("//a[@class='uk-button uk-button-orange uk-button-cancel uk-icon uk-align-right uk-margin-remove-bottom']");

            await Task.Delay(5 * 60 * 1000);
            await _page.ClickAsync("(//button[@class='dt-button'])[1]");
              await Task.Delay(5 * 60 * 1000);
              await _page.ClickAsync("(//button[@class='dt-button'])[1]");
                await Task.Delay(5 * 60 * 1000);
                await _page.ClickAsync("(//button[@class='dt-button'])[1]");
                  await Task.Delay(5 * 60 * 1000);
                  await _page.ClickAsync("(//button[@class='dt-button'])[1]");
                  await _page.IsEnabledAsync("//span[text()='All documents generated']");

        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
