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

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships$", RegexOptions.None, regexMaxTimeOut))
                .UsingGet()
                .WithParam(MatchSearchLocationManchester)
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("search-apprentices-index-location.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships$", RegexOptions.None, regexMaxTimeOut))
                .UsingGet()
                .WithParam(MatchSearchLocationCoventry)
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("search-apprentices-index-no-location-found.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships$", RegexOptions.None, regexMaxTimeOut))
                .UsingGet()
                .WithParam(MatchCandidateId)
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("search-apprentices-authenticated-index.json"));
            
            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships$", RegexOptions.None, regexMaxTimeOut))
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
                    .WithParam(MatchCandidateIdCompleteProfile)
                    .WithParam(MatchLocationParamCoventry)
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("search-apprenticeships-results-signed-in.json"));
            
            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/searchapprenticeships/searchResults", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("search-apprenticeships-results.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/VACABC1000012484", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(404)
                        .WithHeader("Content-Type", "application/json"));
            
            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/VAC\\d{10}$", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("search-vacancy-details.json"));
            
            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/4252449", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("search-nhs-vacancy-details.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/nhs/4252449", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("search-nhs-vacancy-details.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/VACC9348-24-3199", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("search-nhs-vacancy-details.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/nhs/VACC9348-24-3199", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("search-nhs-vacancy-details.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/vacancies/VAC1000012333", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("closed-vacancy-details.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"/candidates/{Constants.CandidateIdWithApplications}", RegexOptions.None, regexMaxTimeOut))
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

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"/vacancies/{Constants.NewVacancyReference}", RegexOptions.None, regexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBodyFromFile("post-application-details-new.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"/vacancies/{Constants.ExistingVacancyReference}", RegexOptions.None, regexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBodyFromFile("post-application-details-existing.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/users/\\S+/user-name", RegexOptions.None, regexMaxTimeOut))
                .UsingGet())
                .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBodyFromFile("get-user-name.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/users/\\S+/date-of-birth", RegexOptions.None, regexMaxTimeOut))
                .UsingGet())
                .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBodyFromFile("get-user-date-of-birth.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/users/\\S+/postcode-address", RegexOptions.None, regexMaxTimeOut))
                .UsingGet())
                .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBodyFromFile("get-postcode-address.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/users/\\S+/select-address", RegexOptions.None, regexMaxTimeOut))
                .UsingGet())
                .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBodyFromFile("get-select-address.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/saved-searches/\\S+/unsubscribe", RegexOptions.None, regexMaxTimeOut))
                .UsingGet())
                .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBodyFromFile("get-confirm-unsubscribe.json"));
            
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, "/saved-searches/unsubscribe", RegexOptions.None, regexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

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
        private static bool MatchLocationParamCoventry(IDictionary<string, WireMockList<string>> arg)
        {
            return arg.ContainsKey("location") && arg["location"].Count != 0 && arg["location"][0].Equals("Coventry", StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool MatchCandidateId(IDictionary<string, WireMockList<string>> arg)
        {
            return arg.ContainsKey("candidateId") && arg["candidateId"].Count != 0 && Guid.TryParse(arg["candidateId"][0], out _);
        }

        private static bool MatchCandidateIdCompleteProfile(IDictionary<string, WireMockList<string>> arg)
        {
            return arg.ContainsKey("candidateId") && arg["candidateId"].Count != 0 && arg["candidateId"][0].Equals(Constants.CandidateIdWithApplications, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
