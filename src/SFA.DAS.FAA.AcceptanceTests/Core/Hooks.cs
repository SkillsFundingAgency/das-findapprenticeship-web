using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Options;
using Reqnroll;
using SFA.DAS.FAA.AcceptanceTests.Pages;
using SFA.DAS.FAA.MockServer.MockServerBuilder;
using SFA.DAS.FAA.Web.Controllers;
using WireMock.Server;

namespace SFA.DAS.FAA.AcceptanceTests.Core;

[Binding]
public class Hooks
{
    private static IWireMockServer? _staticApiServer;
    private static TestServer? _server;
    
    [BeforeScenario]
    public static void BeforeScenario(IOptions<AppSettings> appSettings)
    {
        if (appSettings.Value.Environment != Environments.Development)
        {
            return;
        }

        // --------------------------
        // Local testing
        // --------------------------
        
        // Start the mock server
        _staticApiServer = MockApiServer.Start();
        
        // Start the web project
        var webApp = new CustomWebApplicationFactory<SearchApprenticeshipsController>().WithWebHostBuilder(c => c.UseEnvironment("IntegrationTest").UseConfiguration(ConfigBuilder.GenerateConfiguration()));
        _server = webApp.Server;
    }

    [AfterScenario]
    public static void AfterScenario(IOptions<AppSettings> appSettings)
    {
        if (appSettings.Value.Environment != Environments.Development)
        {
            return;
        }
        
        _staticApiServer?.Stop();
        _staticApiServer?.Dispose();
        _server?.Dispose();
    }
    
    [BeforeScenario("Authenticated")]
    public static async Task AuthenticatedUser(ITestContext context, SignInPage signInPage)
    {
        await signInPage.VisitAsync();
        await signInPage.SignInAsync();
    }
}