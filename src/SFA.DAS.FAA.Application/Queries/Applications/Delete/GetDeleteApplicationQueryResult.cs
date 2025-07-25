using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Applications.Delete;

public class GetDeleteApplicationQueryResult
{
    public static readonly GetDeleteApplicationQueryResult None = new() { ApplicationId = Guid.Empty };

    public List<Address> Addresses { get; init; } = [];
    public Guid ApplicationId { get; init; }
    public DateTime ApplicationStartDate { get; init; }
    public ApprenticeshipTypes ApprenticeshipType { get; init; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public string EmployerName { get; init; } = string.Empty;
    public DateTime? VacancyClosedDate { get; init; }
    public DateTime VacancyClosingDate { get; init; }
    public string VacancyTitle { get; init; } = string.Empty;
}