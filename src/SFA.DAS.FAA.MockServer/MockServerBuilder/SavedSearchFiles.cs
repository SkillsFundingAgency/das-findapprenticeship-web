using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Types;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    [ExcludeFromCodeCoverage]
    public static class SavedSearchFiles
    {
        private const string BaseFilePath = "SavedSearches";
        private static readonly TimeSpan RegexMaxTimeOut = TimeSpan.FromSeconds(3);

        public static WireMockServer WithSavedSearchFiles(this WireMockServer server)
        {
            // save a search
            server
                .Given(
                    Request
                        .Create()
                        .WithPath(s => Regex.IsMatch(s, "searchapprenticeships/saved-search", RegexOptions.None, RegexMaxTimeOut))
                        .WithParam(MatchCandidateId)
                        .WithParam(MatchId)
                        .UsingPost()
                )
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(201)    
                );
            
            // get my saved searches
            server
                .Given(
                    Request
                        .Create()
                        .WithPath(s => Regex.IsMatch(s, "users/(.*)/saved-searches", RegexOptions.None, RegexMaxTimeOut))
                        .UsingGet()
                )
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/get-saved-searches.json")
                );
            
            server
                .Given(
                    Request
                        .Create()
                        .WithPath(s => Regex.IsMatch(s, "users/(.*)/saved-searches/(.*)/delete", RegexOptions.None, RegexMaxTimeOut))
                        .UsingPost()
                )
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(200)
                );
            
            return server;
        }
        
        private static bool MatchCandidateId(IDictionary<string, WireMockList<string>> arg)
        {
            return arg.ContainsKey("candidateId") && arg["candidateId"].Count != 0 && Guid.TryParse(arg["candidateId"][0], out _);
        }
        
        private static bool MatchId(IDictionary<string, WireMockList<string>> arg)
        {
            return arg.ContainsKey("id") && arg["id"].Count != 0 && Guid.TryParse(arg["id"][0], out _);
        }
    }
}