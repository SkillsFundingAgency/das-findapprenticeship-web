using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Commands.SavedSearches.DeleteSavedSearch
{
    public class UnsubscribeSavedSearchCommandHandler(IApiClient apiClient) : IRequestHandler<UnsubscribeSavedSearchCommand, Unit>
    {
        public async Task<Unit> Handle(UnsubscribeSavedSearchCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new PostSavedSearchUnsubscribeApiRequest(request.SavedSearchId);

            await apiClient.Post(apiRequest);
            return Unit.Value;
        }
    }
}
