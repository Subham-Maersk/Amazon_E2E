using NUnit.Framework;
using System.Threading.Tasks;
using Amazon_E2E_copy.Helpers;

namespace Amazon_E2E_copy.Tests
{
    public class LaunchTest : BaseTest
    {
        private Method _method;

        [Test, Category("Custom")] 
        public async Task LaunchTestAsync()
        {
            _method = new Method(_page); 

            // await _method.NavigateTo(_customUrl);
            // await _method.LoginAsync("adarsh.k@maersk.com", "ho6!NX4x2edb%S8CeJgW2t");  
            // await _method.NavigateToCustomsToolsAsync(); 
            // await _method.SelectDateAndAirportsAsync(); 
            // await _method.DispatchAsync("AMSLAXWSSF815XUYXW000001088"); 
            // await _method.VerifyDocumentsGeneratedAsync(); 
        }
    }
}
