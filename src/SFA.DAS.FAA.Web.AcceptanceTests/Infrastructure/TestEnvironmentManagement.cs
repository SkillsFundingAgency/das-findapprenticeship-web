using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.MockServer;
using SFA.DAS.FAA.Web.Controllers;
using TechTalk.SpecFlow;
using WireMock.Server;


namespace SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

[Binding]
public sealed class TestEnvironmentManagement
{
    private readonly ScenarioContext _context;
    private static HttpClient _staticClient;
    private static IWireMockServer _staticApiServer;
    private Mock<IApiClient> _mockApiClient;
    private static TestServer _server;

    public TestEnvironmentManagement(ScenarioContext context)
    {
        _context = context;
    }

    [BeforeScenario("WireMockServer")]
    public void StartWebApp()
    {
        _staticApiServer = MockApiServer.Start();
        var webApp = new CustomWebApplicationFactory<SearchApprenticeshipsController>()
            .WithWebHostBuilder(c=>c.UseEnvironment("IntegrationTest").UseConfiguration(ConfigBuilder.GenerateConfiguration()));
        _server = webApp.Server;
        _staticClient = _server
            .CreateClient();
        _context.Set(_server, ContextKeys.TestServer);
        _context.Set(_staticClient,ContextKeys.HttpClient);
    }

    [BeforeScenario("MockApiClient")]
    public void StartWebAppWithMockApiClient()
    {
        _mockApiClient = new Mock<IApiClient>();

        _mockApiClient.Setup(x => x.Get<SearchApprenticeshipsApiResponse>(It.IsAny<GetSearchApprenticeshipsIndexApiRequest>()))
            .ReturnsAsync(new SearchApprenticeshipsApiResponse());

        _mockApiClient.Setup(x => x.Get<BrowseByInterestsApiResponse>(It.IsAny<GetBrowseByInterestsApiRequest>()))
            .ReturnsAsync(new BrowseByInterestsApiResponse());

        _server = new TestServer(new WebHostBuilder()
            .ConfigureTestServices(services => ConfigureTestServices(services, _mockApiClient))
            .UseEnvironment(Environments.Development)
            .UseStartup<SearchApprenticeshipsController>()
            .UseConfiguration(ConfigBuilder.GenerateConfiguration()));

        _staticClient = _server.CreateClient();

        _context.Set(_mockApiClient, ContextKeys.MockApiClient);
        _context.Set(_staticClient, ContextKeys.HttpClient);
    }

    [AfterScenario("WireMockServer")]
    public void StopEnvironment()
    {
        _server.Dispose();
        _staticApiServer?.Stop();
        _staticApiServer?.Dispose();
        _staticClient?.Dispose();
    }

    [AfterScenario("MockApiClient")]
    public void StopTestEnvironment()
    {
        _server.Dispose();
        _staticClient?.Dispose();
    }

    private void ConfigureTestServices(IServiceCollection serviceCollection, Mock<IApiClient> mockApiClient)
    {
        foreach (var descriptor in serviceCollection.Where(
            d => d.ServiceType ==
                 typeof(IApiClient)).ToList())
        {
            serviceCollection.Remove(descriptor);
        }
        serviceCollection.AddSingleton(mockApiClient);
        serviceCollection.AddSingleton(mockApiClient.Object);
    }
}