using MediatR;

namespace SFA.DAS.FAA.Application.Commands.SavedSearches.DeleteSavedSearch
{
    public class UnsubscribeSavedSearchCommand : IRequest
    {
        public Guid SavedSearchId { get; set; }
    }
}
