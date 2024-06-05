using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
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

            return server;
        }
    }
}
