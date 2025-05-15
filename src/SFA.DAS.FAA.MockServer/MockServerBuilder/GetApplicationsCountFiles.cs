using System.Diagnostics.CodeAnalysis;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    [ExcludeFromCodeCoverage]
    public static class GetApplicationsCountFiles
    {
        private const string BaseFilePath = "Applications";
        private const string BaseRoute = "/applications";

        public static WireMockServer WithGetApplicationsCountFiles(this WireMockServer server)
        {
            server
                .Given(
                    Request
                        .Create()
                        .WithPath($"{BaseRoute}/count")
                        .WithParam("candidateId", new WildcardMatcher("*"))
                        .WithParam("status", new WildcardMatcher("*"))
                        .UsingGet()
                )
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/get-applications-count.json")
                );
            return server;
        }
    }
}
