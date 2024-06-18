using System.Diagnostics.CodeAnalysis;
using WireMock.Logging;
using WireMock.Net.StandAlone;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder;

[ExcludeFromCodeCoverage]
public static class MockApiServer
{
    public static IWireMockServer Start()
    {
        var settings = new WireMockServerSettings
        {
            Port = 5027,
            Logger = new WireMockConsoleLogger()
        };

        var server = StandAloneApp.Start(settings)
            .WithSearchApprenticeshipsFiles()
            .WithNewApplicationFiles()
            .WithExistingApplicationFiles()
            .WithApplicationsFiles()
            .WithCreateAccountFiles();

        return server;
    }
}
