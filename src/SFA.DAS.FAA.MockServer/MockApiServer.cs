using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using WireMock.Logging;
using WireMock.Net.StandAlone;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;

namespace SFA.DAS.FAA.MockServer;

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

        var server = StandAloneApp.Start(settings);

        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships"))
            .UsingGet()
            .WithParam(MatchSearchLocationManchester)
        ).RespondWith(
            Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("search-apprentices-index-location.json"));
        
        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships"))
            .UsingGet()
            .WithParam(MatchSearchLocationCoventry)
        ).RespondWith(
            Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("search-apprentices-index-no-location-found.json"));
        
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
        
        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/browsebyinterestslocation"))
                .WithParam(MatchSearchLocationCoventry)
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody("{}"));
        
        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/browsebyinterestslocation"))
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("browse-location-search.json"));

        


        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/searchResults"))
                .WithParam(MatchLocationParamManchester)
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("search-apprenticeships-no-results.json")); 

        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/searchResults"))
                .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("search-apprenticeships-results.json"));

        server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/1000012013"))
                .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("search-vacancy-details.json"));

        return server;
    }

    private static bool MatchSearchLocationCoventry(IDictionary<string, WireMockList<string>> arg)
    {
        return arg.ContainsKey("locationSearchTerm") && arg["locationSearchTerm"].Count != 0 && arg["locationSearchTerm"][0].Equals("Coventry", StringComparison.CurrentCultureIgnoreCase);        
    }
    private static bool MatchSearchLocationManchester(IDictionary<string, WireMockList<string>> arg)
    {
        return arg.ContainsKey("locationSearchTerm") && arg["locationSearchTerm"].Count != 0 && arg["locationSearchTerm"][0].Equals("Manchester", StringComparison.CurrentCultureIgnoreCase);        
    }

    private static bool MatchLocationParamManchester(IDictionary<string, WireMockList<string>> arg)
    {
        return arg.ContainsKey("location") && arg["location"].Count != 0 && arg["location"][0].Equals("Manchester", StringComparison.CurrentCultureIgnoreCase);
    }
}
