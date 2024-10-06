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
            await _page.GotoAsync("https://www.google.com"); 
            Assert.AreEqual("Google", await _page.TitleAsync()); 

            
            //await Task.Delay(20 * 60 * 1000);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
