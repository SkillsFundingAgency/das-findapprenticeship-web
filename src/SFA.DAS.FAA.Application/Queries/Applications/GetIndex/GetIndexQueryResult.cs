using SFA.DAS.FAA.Domain.Applications.GetApplications;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetIndex;

public class GetIndexQueryResult
{
    public bool ShowAccountRecoveryBanner { get; set; }
    public List<Application> Applications { get; set; } = [];

    public class Application
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string VacancyReference { get; set; }
        public string EmployerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? WithdrawnDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public ApplicationStatus Status { get; set; }
        public string ResponseNotes { get; set; }
    }

    public static implicit operator GetIndexQueryResult(GetApplicationsApiResponse source)
    {
        return new GetIndexQueryResult
        {
            ShowAccountRecoveryBanner = source.ShowAccountRecoveryBanner,
            Applications = source.Applications.Select(x => new Application
            {
                Id = x.Id,
                Title = x.Title,
                VacancyReference = x.VacancyReference,
                EmployerName = x.EmployerName,
                CreatedDate = x.CreatedDate,
                ClosingDate = x.ClosingDate,
                SubmittedDate = x.SubmittedDate,
                ResponseDate = x.ResponseDate,
                Status = x.Status,
                ResponseNotes = x.ResponseNotes,
                WithdrawnDate = x.WithdrawnDate
            }).ToList()
        };
    }
}