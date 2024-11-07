using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Commands.SavedSearches.DeleteSavedSearch
{
    public class UnsubscribeSavedSearchCommandHandler
    {
        private readonly IApiClient _apiClient;

        public UnsubscribeSavedSearchCommandHandler(IApiClient apiClient) => _apiClient = apiClient;

        public async Task<Unit> Handle(UnsubscribeSavedSearchCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new PostSavedSearchUnsubscribeApiRequest(request.SavedSearchId);

            await _apiClient.PostWithResponseCode(apiRequest);
            return Unit.Value;
        }
    }
}
