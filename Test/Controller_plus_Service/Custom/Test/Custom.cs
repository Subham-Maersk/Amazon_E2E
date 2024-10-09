using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;
using Configuration;
using Amazon_E2E_copy.Helpers;

namespace Amazon_E2E_copy.Tests
{
    public class LaunchTest
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private string _customUrl;
        private Method _method;

        [SetUp]
        public async Task Setup()
        {
            _customUrl = EnvironmentConfig.GetAllUrls()["CUSTOM_E2E"];
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false 
            });
            _page = await _browser.NewPageAsync();
            _method = new Method(_page); 
        }

        [Test, Category("Custom")] 
        public async Task LaunchTestAsync()
        {
            await _page.GotoAsync(_customUrl);
            await _method.LoginAsync("adarsh.k@maersk.com", "ho6!NX4x2edb%S8CeJgW2t"); 
            await _method.NavigateToCustomsToolsAsync(); 
            await _method.SelectDateAndAirportsAsync(); 
            await _method.DispatchAsync("AMSLAXWSSF815XUYXW000001088"); 
            await _method.VerifyDocumentsGeneratedAsync(); 
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
