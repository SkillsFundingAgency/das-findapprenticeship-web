using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Models.Applications;

public class ConfirmDeleteApplicationViewModel
{   
    public required Guid ApplicationId { get; init; }
    public required string ApplicationStartDate { get; init; }
    public required ApprenticeshipTypes ApprenticeshipType { get; init; }
    public required string EmployerName { get; init; }
    public required string VacancyCloseDate { get; init; }
    public required string VacancyTitle { get; init; }
    public required string WorkLocation { get; init; }
}