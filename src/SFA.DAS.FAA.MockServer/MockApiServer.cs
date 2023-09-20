using System.Text.RegularExpressions;
using WireMock.Logging;
using WireMock.Net.StandAlone;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;

namespace SFA.DAS.FAA.MockServer;

public static class MockApiServer
{
    public static IWireMockServer Start()
    {
        var settings = new WireMockServerSettings
        {
            Port = 5003,
            Logger = new WireMockConsoleLogger()
        };
        
        var server = StandAloneApp.Start(settings);
        
        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s,"/searchapprenticeships"))
            .UsingGet()
        ).RespondWith(
            Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("search-apprentices-index.json"));
        
        server.Given(Request.Create().WithPath(s=>Regex.IsMatch(s, @"/locations")).UsingGet()).RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBodyFromFile("locations.json"));
        return server;
    }
}
