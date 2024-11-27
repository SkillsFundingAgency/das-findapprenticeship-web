using MediatR;

namespace SFA.DAS.FAA.Application.Commands.SavedSearches.DeleteSavedSearch
{
    public class UnsubscribeSavedSearchCommand : IRequest<Unit>
    {
        public Guid SavedSearchId { get; set; }
    }
}
