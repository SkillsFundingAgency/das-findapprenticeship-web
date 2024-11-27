using MediatR;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches.GetConfirmUnsubscribe;

public class GetConfirmUnsubscribeQuery : IRequest<GetConfirmUnsubscribeResult>
{
    public Guid SavedSearchId { get; set; }
}