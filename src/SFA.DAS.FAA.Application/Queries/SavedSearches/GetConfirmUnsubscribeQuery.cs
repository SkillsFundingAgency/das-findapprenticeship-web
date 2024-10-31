using MediatR;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches;

public class GetConfirmUnsubscribeQuery : IRequest<GetConfirmUnsubscribeResult>
{
    public Guid SavedSearchId { get; set; }
}