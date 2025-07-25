using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Applications.DeleteApplication;

public class GetConfirmDeleteApplicationApiResponse
{
    public Address? Address { get; set; }
    public Guid ApplicationId { get; init; }
    public DateTime? ApplicationStartDate { get; init; }
    public ApprenticeshipTypes? ApprenticeshipType { get; init; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public required string EmployerName { get; init; }
    public List<Address>? OtherAddresses { get; set; } = [];
    public DateTime? VacancyClosedDate { get; init; }
    public DateTime? VacancyClosingDate { get; init; }
    public required string VacancyTitle { get; init; }
}