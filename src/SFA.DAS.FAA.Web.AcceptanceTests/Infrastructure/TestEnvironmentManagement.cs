using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.MockServer.MockServerBuilder;
using SFA.DAS.FAA.Web.Controllers;
using TechTalk.SpecFlow;
using WireMock.Server;
using Constants = SFA.DAS.FAA.Web.AcceptanceTests.Data.Constants;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

[Binding]
public sealed class TestEnvironmentManagement
{
    private readonly ScenarioContext _context;
    private static TestHttpClient _testHttpClient;
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

        _testHttpClient = new TestHttpClient(_server);

        _context.Set(_server, ContextKeys.TestServer);
        _context.Set(_testHttpClient, ContextKeys.TestHttpClient);
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

        _context.Set(_mockApiClient, ContextKeys.MockApiClient);
    }

    [BeforeScenario("NewApplication")]
    public async Task NewApplication()
    {
        _context.Set(Constants.NewVacancyReference, ContextKeys.VacancyReference);
        _context.Set(Constants.NewApplicationId, ContextKeys.ApplicationId);
    }

    [BeforeScenario("ExistingApplication")]
    public async Task ExistingApplication()
    {
        _context.Set(Constants.ExistingVacancyReference, ContextKeys.VacancyReference);
        _context.Set(Constants.ExistingApplicationId, ContextKeys.ApplicationId);
    }

    [BeforeScenario("AuthenticatedUser")]
    public async Task AuthenticatedUser()
    {
        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);

        var formData = new Dictionary<string, string>
        {
            { "Id", MockServer.Constants.CandidateIdWithApplications },
            { "Email", "test@test.com" },
            { "MobilePhone", "12345 67890" }
        };

        var content = new FormUrlEncodedContent(formData);

        await client.PostAsync("/account-details", content);
    }

    [BeforeScenario("AuthenticatedUserWithIncompleteSetup")]
    public async Task AuthenticatedUserWithIncompleteSetup()
    {
        var client = _context.Get<TestHttpClient>(ContextKeys.TestHttpClient);

        var formData = new Dictionary<string, string>
        {
            { "Id", MockServer.Constants.CandidateIdIncomplete },
            { "Email", "test@test.com" },
            { "MobilePhone", "12345 67890" }
        };

        var content = new FormUrlEncodedContent(formData);

        await client.PostAsync("/account-details", content);
    }

    [AfterScenario("WireMockServer")]
    public void StopEnvironment()
    {
        _server.Dispose();
        _staticApiServer?.Stop();
        _staticApiServer?.Dispose();
        _testHttpClient?.Dispose();
    }

    [AfterScenario("MockApiClient")]
    public void StopTestEnvironment()
    {
        _server.Dispose();
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