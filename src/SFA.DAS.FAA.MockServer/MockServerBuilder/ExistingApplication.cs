using System.Text.RegularExpressions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    public static class ExistingApplication
    {
        public static WireMockServer WithExistingApplicationFiles(this WireMockServer server)
        {
            var existingApplicationRoute = $"/applications/676476cc-525a-4a13-8da7-cf36345e6f61";
            var regexMaxTimeOut = TimeSpan.FromSeconds(3);

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{existingApplicationRoute}", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("Apply/ExistingApplication/get-application.json"));

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{existingApplicationRoute}/jobs", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("Apply/ExistingApplication/Jobs/get-jobs.json"));

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{existingApplicationRoute}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("Apply/ExistingApplication/Jobs/get-job.json"));

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{existingApplicationRoute}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae/delete", RegexOptions.None, regexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("Apply/ExistingApplication/Jobs/get-delete-job.json"));

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, "/vacancies/1000012013", RegexOptions.None, regexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("Apply/ExistingApplication/post-application-details.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{existingApplicationRoute}/\\S+/work-history", RegexOptions.None, regexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            return server;
        }
    }
}
