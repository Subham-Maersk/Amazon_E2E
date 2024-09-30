// class Amazon
// {
//     static async Task Main(string[] Login)
//     {
//         // Fetch JWT token from JwtService
//         string jwtToken = await JwtService.GetJwtTokenAsync();
//         Console.WriteLine($"JWT Token: {jwtToken}");

//         var playwright = await Playwright.CreateAsync();
//         var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
//         var context = await browser.NewContextAsync(new BrowserNewContextOptions
//         {
//             ExtraHTTPHeaders = new Dictionary<string, string>
//             {
//                 { "Authorization", $"Bearer {jwtToken}" }
//             }
//         });

//         // Example: Uncomment to open a page
//         // var page = await context.NewPageAsync();
//         // await page.GotoAsync("https://google.com");

//         await browser.CloseAsync();
//     }
// }