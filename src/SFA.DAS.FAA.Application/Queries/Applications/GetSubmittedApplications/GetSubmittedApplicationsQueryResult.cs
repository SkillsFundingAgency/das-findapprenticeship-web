using SFA.DAS.FAA.Domain.Applications.GetSubmittedApplications;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetSubmittedApplications
{
    public record GetSubmittedApplicationsQueryResult
    {
        public List<Application> SubmittedApplications { get; set; } = [];

        public static implicit operator GetSubmittedApplicationsQueryResult(GetSubmittedApplicationsApiResponse source)
        {
            return new GetSubmittedApplicationsQueryResult
            {
                SubmittedApplications = source.SubmittedApplications.Select(app => (Application)app).ToList()
            };
        }

        public class Application
        {
            public Guid Id { get; set; }
            public string? Title { get; set; }
            public string? VacancyReference { get; set; }
            public string? EmployerName { get; set; }
            public string? City { get; set; }
            public string? Postcode { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? SubmittedDate { get; set; }
            public DateTime ClosingDate { get; set; }
            public ApplicationStatus Status { get; set; }

            public static implicit operator Application(GetSubmittedApplicationsApiResponse.SubmittedApplication source)
            {
                return new Application
                {
                    Id = source.Id,
                    Title = source.Title,
                    VacancyReference = source.VacancyReference,
                    EmployerName = source.EmployerName,
                    CreatedDate = source.CreatedDate,
                    ClosingDate = source.ClosingDate,
                    SubmittedDate = source.SubmittedDate,
                    Status = (ApplicationStatus)source.Status,
                    City = source.City,
                    Postcode = source.Postcode,
                };
            }
        }
    }
}
