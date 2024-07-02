using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    [ExcludeFromCodeCoverage]
    public static class SavedVacanciesFiles
    {
        private static readonly string BaseRoute = $"/saved-vacancies";
        private static readonly TimeSpan RegexMaxTimeOut = TimeSpan.FromSeconds(3);
        private static readonly string EmptyStateBaseFilePath = $"SavedVacancies/EmptyState";
        private static readonly string PopulatedStateBaseFilePath = $"SavedVacancies/PopulatedState";

        public static WireMockServer WithSavedVacanciesFiles(this WireMockServer server)
        {
            return server.WithNoApplicationsFiles().WithExistingApplicationsFiles();
        }

        private static WireMockServer WithExistingApplicationsFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}", RegexOptions.None, RegexMaxTimeOut))
                    .WithParam("candidateId", Constants.CandidateIdWithApplications)
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{PopulatedStateBaseFilePath}/get-saved-vacancies.json"));

            return server;
        }


        private static WireMockServer WithNoApplicationsFiles(this WireMockServer server)
        {
            return server;
        }
    }
}
