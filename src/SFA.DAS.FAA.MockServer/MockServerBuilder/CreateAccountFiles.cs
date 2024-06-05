using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Web;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    [ExcludeFromCodeCoverage]
    public static class CreateAccountFiles
    {
        private static readonly string BaseFilePath = $"CreateAccount/";
        private static readonly TimeSpan RegexMaxTimeOut = TimeSpan.FromSeconds(3);

        public static WireMockServer WithCreateAccountFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"/candidates/{Constants.CandidateIdIncomplete}", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPut())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(202)
                        .WithBodyFromFile("put-candidate-incomplete.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"/candidates/create-account", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet()
                    .WithParam("candidateId", Constants.CandidateIdIncomplete)
                ).RespondWith(
                    Response.Create()
                        .WithStatusCode(202)
                        .WithBodyFromFile($"{BaseFilePath}/get-inform.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"/candidates/sign-in-to-your-old-account", RegexOptions.None, RegexMaxTimeOut))
                .UsingGet()
                .WithParam("email", "invalid@test.com")
                ).RespondWith(
                Response.Create()
                    .WithStatusCode(202)
                    .WithBodyFromFile($"{BaseFilePath}/get-sign-in-to-your-old-account-failure.json"));

            return server;
        }
    }
}
