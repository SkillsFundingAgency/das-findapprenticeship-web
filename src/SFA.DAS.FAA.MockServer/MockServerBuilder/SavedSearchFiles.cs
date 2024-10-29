using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    [ExcludeFromCodeCoverage]
    public static class SavedSearchFiles
    {
        // private static readonly string BaseFilePath = $"CreateAccount/";
        private static readonly TimeSpan RegexMaxTimeOut = TimeSpan.FromSeconds(3);

        public static WireMockServer WithSavedSearchFiles(this WireMockServer server)
        {
            server
                .Given(
                    Request
                        .Create()
                        .WithPath(s => Regex.IsMatch(s, $"searchapprenticeships/saved-search?candidateId={Constants.CandidateIdWithApplications}", RegexOptions.None, RegexMaxTimeOut))
                        .UsingPost()
                )
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(201)    
                );
            return server;
        }
    }
}
