using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;
using Configuration;

namespace Amazon_E2E_copy.Tests
{
    public abstract class BaseTest
    {
        protected IPlaywright _playwright;
        protected IBrowser _browser;
        protected IPage _page;
        protected string _customUrl;

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
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
