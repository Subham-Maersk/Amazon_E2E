using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using Amazon_E2E_copy.Locators;

namespace Amazon_E2E_copy.Helpers
{
    public class Method
    {
        private readonly IPage _page;

        public Method(IPage page)
        {
            _page = page;
        }

        public async Task NavigateTo(string url)
        {
            await _page.GotoAsync(url);
        }

        public async Task LoginAsync(string username, string password)
        {
            await _page.FillAsync(CustomLocators.UsernameInput, username);
            await _page.FillAsync(CustomLocators.PasswordInput, password);
            await _page.ClickAsync(CustomLocators.SubmitButton);
            await Task.Delay(1200);
        }

        public async Task NavigateToCustomsToolsAsync()
        {
            await _page.ClickAsync(CustomLocators.CustomsToolsLink);
            await _page.ClickAsync(CustomLocators.InternationalCustomsLink);
        }

        public async Task SelectDateAndAirportsAsync()
        {
            await _page.ClickAsync(CustomLocators.CreateButton);
            await _page.ClickAsync(CustomLocators.DayButton);
            await _page.ClickAsync(CustomLocators.ArrivalDateInput);
            await _page.ClickAsync(CustomLocators.DayButton);

            await _page.Locator(CustomLocators.OriginAirportDropdown).SelectOptionAsync(new[] { "AMS (Netherlands)" });
            var selectedValue = await _page.Locator(CustomLocators.OriginAirportDropdown).InputValueAsync();
            Console.WriteLine($"Selected value: {selectedValue}");

            await _page.Locator(CustomLocators.DestinationAirportDropdown).SelectOptionAsync(new[] { "JFK (United States)" });
            var selectedValue2 = await _page.Locator(CustomLocators.DestinationAirportDropdown).InputValueAsync();
            Console.WriteLine($"Selected value2: {selectedValue2}");

            await Task.Delay(1200);
        }

        public async Task DispatchAsync(string dispatchNumber)
        {
            await _page.ClickAsync(CustomLocators.CreateDispatchButton);
            await _page.ClickAsync(CustomLocators.OverpacksDropdown);
            await _page.Locator(CustomLocators.OverpacksDropdown).SelectOptionAsync(new[] { "Insert Overpacks" });
            var selectedValue3 = await _page.Locator(CustomLocators.OverpacksDropdown).InputValueAsync();
            Console.WriteLine($"Selected value: {selectedValue3}");

            await Task.Delay(1200);
            await _page.FillAsync(CustomLocators.DispatchNumbersTextarea, dispatchNumber);
            await _page.ClickAsync(CustomLocators.SendDispatchButton);
            await _page.ClickAsync(CustomLocators.CancelButton);
        }

        public async Task VerifyDocumentsGeneratedAsync()
        {
            for (int i = 0; i < 4; i++)
            {
                await Task.Delay(5 * 60 * 1000);
                await _page.ClickAsync(CustomLocators.Button);
            }

            var isGenerated = await _page.IsEnabledAsync(CustomLocators.DocumentsGeneratedMessage);
            if (isGenerated)
            {
                Console.WriteLine("All documents generated successfully.");
            }
        }
    }
}
