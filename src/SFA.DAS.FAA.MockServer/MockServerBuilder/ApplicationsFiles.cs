using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationsFiles
    {
        private static readonly string BaseRoute = $"/applications";
        private static readonly TimeSpan RegexMaxTimeOut = TimeSpan.FromSeconds(3);
        private static readonly string EmptyStateBaseFilePath = $"Applications/EmptyState";
        private static readonly string PopulatedStateBaseFilePath = $"Applications/PopulatedState";

        public static WireMockServer WithApplicationsFiles(this WireMockServer server)
        {
            return server.WithNoApplicationsFiles().WithExistingApplicationsFiles();
        }

        private static WireMockServer WithExistingApplicationsFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}", RegexOptions.None, RegexMaxTimeOut))
                    .WithParam("candidateId", Constants.CandidateIdWithApplications)
                    .WithParam("status", "Draft")
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{PopulatedStateBaseFilePath}/get-applications.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}", RegexOptions.None, RegexMaxTimeOut))
                    .WithParam("candidateId", Constants.CandidateIdWithApplications)
                    .WithParam("status", "Submitted")
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{PopulatedStateBaseFilePath}/get-applications-submitted.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}", RegexOptions.None, RegexMaxTimeOut))
                    .WithParam("candidateId", Constants.CandidateIdWithApplications)
                    .WithParam("status", "Successful")
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{PopulatedStateBaseFilePath}/get-applications-successful.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}", RegexOptions.None, RegexMaxTimeOut))
                    .WithParam("candidateId", Constants.CandidateIdWithApplications)
                    .WithParam("status", "Unsuccessful")
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{PopulatedStateBaseFilePath}/get-applications-unsuccessful.json"));


            return server;
        }


        private static WireMockServer WithNoApplicationsFiles(this WireMockServer server)
        {
            //server.Given(Request.Create().WithPath(s =>
            //            Regex.IsMatch(s, $"{BaseRoute}", RegexOptions.None, RegexMaxTimeOut))
            //        .WithParam("candidateId", Constants.CandidateIdWithNoApplications)
            //        .UsingGet())
            //    .RespondWith(
            //        Response.Create()
            //            .WithStatusCode(200)
            //            .WithHeader("Content-Type", "application/json")
            //            .WithBodyFromFile($"{EmptyStateBaseFilePath}/get-applications.json"));

            return server;
        }
    }
}
