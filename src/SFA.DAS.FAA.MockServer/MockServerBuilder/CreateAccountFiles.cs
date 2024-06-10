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


            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"/candidates/sign-in-to-your-old-account", RegexOptions.None, RegexMaxTimeOut))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202)
                    .WithBodyFromFile($"{BaseFilePath}/get-sign-in-to-your-old-account-success.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"users/{Constants.CandidateIdIncomplete}/create-account/user-name", RegexOptions.None, RegexMaxTimeOut))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202)
                    .WithBodyFromFile($"{BaseFilePath}/get-user-name.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s,
                    $"users/{Constants.CandidateIdIncomplete}/create-account/user-name", RegexOptions.None,
                    RegexMaxTimeOut))
                .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"users/{Constants.CandidateIdIncomplete}/create-account/date-of-birth", RegexOptions.None, RegexMaxTimeOut))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202)
                    .WithBodyFromFile($"{BaseFilePath}/get-user-date-of-birth.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"users/{Constants.CandidateIdIncomplete}/create-account/date-of-birth", RegexOptions.None, RegexMaxTimeOut))
                .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"users/create-account/postcode-address", RegexOptions.None, RegexMaxTimeOut))
                .UsingGet()
                .WithParam("postcode", "B61 0UA")
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202)
                    .WithBodyFromFile($"{BaseFilePath}/get-user-postcode-address-success.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"users/create-account/postcode-address", RegexOptions.None, RegexMaxTimeOut))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202)
                    .WithBodyFromFile($"{BaseFilePath}/get-user-postcode-address-failure.json"));


            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"users/{Constants.CandidateIdIncomplete}/create-account/select-address", RegexOptions.None, RegexMaxTimeOut))
                .UsingGet()
                .WithParam("postcode", "B61 0UA")
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202)
                    .WithBodyFromFile($"{BaseFilePath}/get-user-select-address.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"users/{Constants.CandidateIdIncomplete}/create-account/check-answers", RegexOptions.None, RegexMaxTimeOut))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202)
                    .WithBodyFromFile($"{BaseFilePath}/get-user-check-answers.json"));


            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"user/settings", RegexOptions.None, RegexMaxTimeOut))
                .UsingGet()
                .WithParam("candidateId", Constants.CandidateIdWithApplications)
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(202)
                    .WithBodyFromFile($"{BaseFilePath}/get-user-settings.json"));

            return server;
        }
    }
}
