using Microsoft.Playwright;
using System.Threading.Tasks;

namespace TestProject.Pages
{
    public abstract class BasePage
    {
        protected readonly IPage _page;

        public BasePage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateTo(string url)
        {
            await _page.GotoAsync(url);
        }
    }
}
