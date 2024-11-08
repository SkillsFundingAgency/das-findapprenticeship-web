using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Application.Commands.SavedSearches.PostSaveSearch;

public class SaveSearchCommandHandler(IApiClient apiClient) : IRequestHandler<SaveSearchCommand, Unit>
{
    public async Task<Unit> Handle(SaveSearchCommand request, CancellationToken cancellationToken)
    {
        var apiRequest = new PostSaveSearchApiRequest(request.CandidateId,
            request.Id,
            new PostSaveSearchApiRequest.PostSaveSearchApiRequestData(
                request.DisabilityConfident,
                request.Distance,
                request.Location,
                request.SearchTerm,
                request.SelectedLevelIds,
                request.SelectedRouteIds,
                request.SortOrder,
                request.UnSubscribeToken
            )
        );

        await apiClient.PostWithResponseCode(apiRequest);
        return Unit.Value;
    }
}