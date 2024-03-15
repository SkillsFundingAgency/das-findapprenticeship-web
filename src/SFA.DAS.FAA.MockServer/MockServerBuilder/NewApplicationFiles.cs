using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.FAA.MockServer.MockServerBuilder
{
    [ExcludeFromCodeCoverage]
    public static class NewApplication
    {
        private static readonly string BaseRoute = $"/applications/{Constants.NewApplicationId}";
        private static readonly TimeSpan RegexMaxTimeOut = TimeSpan.FromSeconds(3);
        private static readonly string BaseFilePath = $"Apply/NewApplication";

        public static WireMockServer WithNewApplicationFiles(this WireMockServer server)
        {
            return server
                .WithApplicationFiles()
                .WithJobsFiles()
                .WithTrainingCoursesFiles()
                .WithVolunteeringAndWorkExperienceFiles()
                .WithDisabilityConfidentFiles()
                .WithQualificationsFiles();
        }

        private static WireMockServer WithApplicationFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"/vacancies/{Constants.NewVacancyReference}", RegexOptions.None,
                            RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/post-application-details.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, BaseRoute, RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/get-application.json"));

            return server;
        }

        private static WireMockServer WithJobsFiles(this WireMockServer server)
        {
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/jobs", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Jobs/get-jobs.json"));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/\\S+/work-history", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/jobs", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Jobs/post-add-job.json"));

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
                        Regex.IsMatch(s, $"{BaseRoute}/\\S+/training-courses", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty));

            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/trainingcourses", RegexOptions.None, RegexMaxTimeOut))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/TrainingCourses/post-add-trainingcourse.json"));

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
            
            server.Given(Request.Create().WithPath(s =>
                        Regex.IsMatch(s, $"{BaseRoute}/qualifications/\\S*/add", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Qualifications/add-qualification.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, $"{BaseRoute}/qualifications/add/select-type", RegexOptions.None, RegexMaxTimeOut))
                    .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile($"{BaseFilePath}/Qualifications/qualification-types.json"));
            
            return server;
        }
    }
}
