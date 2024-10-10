using Microsoft.Playwright;
using System.Threading.Tasks;

namespace TestProject.Helpers
{
    public class UIHelper
    {
        private IPage _page;

        public UIHelper(IPage page)
        {
            _page = page;
        }

        // text field 
        public async Task FillTextField(string selector, string value)
        {
            var element = _page.Locator(selector);
            await element.FillAsync(value); 
        }

        //dropdown text
        public async Task SelectDropdownByText(string selector, string value)
        {
            var element = _page.Locator(selector);
            await element.SelectOptionAsync(new SelectOptionValue { Label = value });  
        }

        // dropdown value
        public async Task SelectDropdownByValue(string selector, string value)
        {
            var element = _page.Locator(selector);
            await element.SelectOptionAsync(value);  
        }

        // click
        public async Task ClickButton(string selector)
        {
            var button = _page.Locator(selector);
            await button.ClickAsync(); 
        }

        public async Task TakeScreenshotAsync(string filePath)
        {
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "Screenshot.png" });
        }
    }
}
