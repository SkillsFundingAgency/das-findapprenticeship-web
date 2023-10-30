using System.Text.RegularExpressions;
using WireMock.Logging;
using WireMock.Matchers;
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

        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships"))
            .UsingGet()
        ).RespondWith(
            Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("search-apprentices-index.json"));

        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, @"/locations")).UsingGet()).RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBodyFromFile("locations.json"));

        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/browsebyinterests"))
            .UsingGet()
            ).RespondWith(
            Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("routes.json"));

        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/locations/searchbylocation"))
            .UsingGet())
            .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBodyFromFile("location-search.json"));
        
        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/locations/searchbylocation"))
                .WithParam(MatchLocationParamCoventry)
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody("{\n  \"locations\": []\n}"));
        
        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/locations/searchbylocation"))
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("location-search.json"));
        
        

        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/locations/geopoint"))
        .UsingGet())
            .RespondWith(
            Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBodyFromFile("geopoint.json"));

        return server;
    }
    
    private static bool MatchLocationParamCoventry(IDictionary<string, WireMockList<string>> arg)
    {
        return arg.ContainsKey("searchTerm") && arg["searchTerm"].Count != 0 && arg["searchTerm"][0].Equals("Coventry", StringComparison.CurrentCultureIgnoreCase);        
    }
}
