using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Types;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    [ExcludeFromCodeCoverage]
    public static class ExistingApplication
    {
        private static readonly string BaseRoute = $"/applications/{Constants.ExistingApplicationId}";
        private static readonly TimeSpan RegexMaxTimeOut = TimeSpan.FromSeconds(3);
        private static readonly string BaseFilePath = $"Apply/ExistingApplication";

        public static WireMockServer WithExistingApplicationFiles(this WireMockServer server)
        {
            return server
                .WithApplicationFiles()
                .WithJobsFiles()
                .WithTrainingCoursesFiles()
                .WithVolunteeringAndWorkExperienceFiles()
                .WithDisabilityConfidentFiles()
                .WithQualificationsFiles();

            return server;
        }

        private static WireMockServer WithApplicationFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{BaseRoute}", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/get-application.json"));

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, "/vacancies/1000012013", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/post-application-details.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/\\S+/work-history", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            return server;
        }

        private static WireMockServer WithJobsFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{BaseRoute}/jobs", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Jobs/get-jobs.json"));

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{BaseRoute}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Jobs/get-job.json"));

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{BaseRoute}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{BaseRoute}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae/delete", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Jobs/get-delete-job.json"));

            server.Given(Request.Create().WithPath(s
                        => Regex.IsMatch(s, $"{BaseRoute}/jobs/0dfaedf4-e8a0-4181-b08d-17b2d2e997ae/delete", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            return server;
        }

        private static WireMockServer WithTrainingCoursesFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/trainingcourses", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/TrainingCourses/get-trainingcourses.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/trainingcourses/\\S+", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/TrainingCourses/get-trainingcourse.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/trainingcourses/\\S+", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/trainingcourses/\\S+/delete", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/TrainingCourses/get-delete-trainingcourse.json"));


            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/trainingcourses/\\S+/delete", RegexOptions.None,
                            RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            return server;
        }

        private static WireMockServer WithVolunteeringAndWorkExperienceFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/volunteeringorworkexperience", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/WorkExperience/get-workexperiences.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/\\S+/volunteering-and-work-experience", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));


            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/volunteeringorworkexperience/\\S+/delete",
                            RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/WorkExperience/get-delete-workexperience.json"));

            return server;
        }

        private static WireMockServer WithDisabilityConfidentFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/disability-confident", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/DisabilityConfident/get-disabilityconfident.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/disability-confident", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            return server;
        }


        private static WireMockServer WithQualificationsFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/qualifications", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Qualifications/get-qualifications.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/qualifications", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"{BaseRoute}/qualifications/add/select-type", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Qualifications/qualification-types.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"{BaseRoute}/qualifications/delete/2cb6af9b-77f9-4f47-af64-253a8bcc87bb", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Qualifications/get-delete-qualifications-single.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"{BaseRoute}/qualifications/delete/20d1923f-25b4-4a37-8580-d04643cf1fba", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Qualifications/get-delete-qualifications-multiple.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/qualifications/delete/20d1923f-25b4-4a37-8580-d04643cf1fba", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/qualifications/delete/20d1923f-25b4-4a37-8580-d04643cf1fba", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"{BaseRoute}/qualifications/f0dd327d-8888-4a00-a529-ea8e35d38bb5/modify", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Qualifications/modify-qualification-multiple.json"));
            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"{BaseRoute}/qualifications/20d1923f-25b4-4a37-8580-d04643cf1fba/modify", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet()
                    .WithParam(MatchQualificationIdParam))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Qualifications/modify-qualification-single.json"));
            return server;
        }
        
        private static bool MatchQualificationIdParam(IDictionary<string, WireMockList<string>> arg)
        {
            return arg.ContainsKey("qualificationId") && arg["qualificationId"].Count != 0 && arg["qualificationId"][0].Equals("19FC767D-D6B9-4327-9EDE-6B7B83CBB6AD", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
