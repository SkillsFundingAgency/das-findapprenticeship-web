using MediatR;

namespace SFA.DAS.FAA.Application.Queries.GetSearchResults;

public class GetSearchResultsQuery : IRequest<GetSearchResultsResult>
{
    public string? Location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }

    public int? Distance { get; set; }
    public string? SearchTerm { get; set; }
    public int? PageNumber { get; set; } 
    public int? PageSize { get; set; }
}