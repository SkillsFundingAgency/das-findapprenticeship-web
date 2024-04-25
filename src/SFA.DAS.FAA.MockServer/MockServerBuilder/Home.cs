using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Types;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    [ExcludeFromCodeCoverage]
    public static class Home
    {
        public static WireMockServer WithSearchApprenticeshipsFiles(this WireMockServer server)
        {
            var regexMaxTimeOut = TimeSpan.FromSeconds(3);

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships", RegexOptions.None, regexMaxTimeOut))
            .UsingGet()
            .WithParam(MatchSearchLocationManchester)
        ).RespondWith(
            Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("search-apprentices-index-location.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships", RegexOptions.None, regexMaxTimeOut))
                .UsingGet()
                .WithParam(MatchSearchLocationCoventry)
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("search-apprentices-index-no-location-found.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships", RegexOptions.None, regexMaxTimeOut))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("search-apprentices-index.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, @"/locations", RegexOptions.None, regexMaxTimeOut))
                .UsingGet())
                .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("locations.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/browsebyinterests", RegexOptions.None, regexMaxTimeOut))
                .UsingGet()
                ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("routes.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/locations/searchbylocation", RegexOptions.None, regexMaxTimeOut))
                .UsingGet())
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("location-search.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/browsebyinterestslocation", RegexOptions.None, regexMaxTimeOut))
                    .WithParam(MatchSearchLocationCoventry)
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody("{}"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/browsebyinterestslocation", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("browse-location-search.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/searchResults", RegexOptions.None, regexMaxTimeOut))
                    .WithParam(MatchLocationParamManchester)
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("search-apprenticeships-no-results.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/searchResults", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("search-apprenticeships-results.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/\\d+$", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("search-vacancy-details.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/1000012333", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("closed-vacancy-details.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/candidates/\\S+$", RegexOptions.None, regexMaxTimeOut))
                     .UsingPut())
                  .RespondWith(
                     Response.Create()
                         .WithStatusCode(202)
                         .WithBodyFromFile("put-candidate.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/users/\\S+/add-details", RegexOptions.None, regexMaxTimeOut))
                    .UsingPut())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(202)
                    .WithBodyFromFile("put-candidate.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/users/\\S+/add-details", RegexOptions.None, regexMaxTimeOut))
                    .UsingPut())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(202)
                        .WithBodyFromFile("put-candidate.json"));

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
}
