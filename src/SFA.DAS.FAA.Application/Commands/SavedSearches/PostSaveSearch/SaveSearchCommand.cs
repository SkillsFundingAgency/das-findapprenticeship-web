using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.SavedSearches.PostSaveSearch;

public class SaveSearchCommand : IRequest<Unit>
{
    public required Guid Id { get; set; }
    public required Guid CandidateId { get; set; }
    public bool DisabilityConfident { get; set; }
    public int? Distance { get; set; }
    public string? Location { get; set; }
    public string? SearchTerm { get; set; }
    public List<string>? SelectedLevelIds { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    public string? SortOrder { get; set; }
    public string UnSubscribeToken { get; set; } = null!;
    public bool? ExcludeNational { get; set; }
    public List<ApprenticeshipTypes>? ApprenticeshipTypes { get; set; }
}