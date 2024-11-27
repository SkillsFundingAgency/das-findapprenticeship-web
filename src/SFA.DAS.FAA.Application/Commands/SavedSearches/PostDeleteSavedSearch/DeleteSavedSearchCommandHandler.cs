using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Commands.SavedSearches.PostDeleteSavedSearch;

public class DeleteSavedSearchCommandHandler(IApiClient apiClient) : IRequestHandler<DeleteSavedSearchCommand>
{
    public async Task Handle(DeleteSavedSearchCommand request, CancellationToken cancellationToken)
    {
        var apiRequest = new PostDeleteSavedSearchRequest(request.CandidateId, request.Id);
        await apiClient.PostWithResponseCode(apiRequest);
    }
}