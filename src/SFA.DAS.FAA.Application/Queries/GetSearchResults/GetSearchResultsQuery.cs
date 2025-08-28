using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Queries.GetSearchResults;

public class GetSearchResultsQuery : IRequest<GetSearchResultsResult>
{
    public string? Location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    public List<string>? SelectedLevelIds { get; set; }
    public int? Distance { get; set; }
    public string? SearchTerm { get; set; }
    public int? PageNumber { get; set; } 
    public int? PageSize { get; set; }
    public string? Sort { get; set; }
    public bool DisabilityConfident { get; set; }
    public string? CandidateId { get; set; }
    public WageType? SkipWageType { get; set; }
    public bool? ExcludeNational { get; set; }
    public List<ApprenticeshipTypes>? ApprenticeshipTypes { get; set; }
}