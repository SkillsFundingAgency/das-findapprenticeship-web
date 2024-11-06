using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.SavedSearches.SaveSearch;

public class SaveSearchCommandHandler(IApiClient apiClient) : IRequestHandler<SaveSearchCommand, Unit>
{
    public async Task<Unit> Handle(SaveSearchCommand request, CancellationToken cancellationToken)
    {
        var apiRequest = new PostSaveSearchApiRequest(request.CandidateId,
            new PostSaveSearchApiRequest.PostSaveSearchApiRequestData(
                request.DisabilityConfident,
                request.Distance,
                request.Location,
                request.SearchTerm,
                request.SelectedLevelIds,
                request.SelectedRouteIds,
                request.SortOrder
            )
        );

        await apiClient.PostWithResponseCode(apiRequest);
        return Unit.Value;
    }
}