using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Applications.GetApplications;

public class GetApplicationsApiResponse
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
        public DateTime? ClosedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? WithdrawnDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public ApplicationStatus Status { get; set; }
        public string ResponseNotes { get; set; }
    }
}