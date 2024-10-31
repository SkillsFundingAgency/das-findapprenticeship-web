using MediatR;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches;

public class GetConfirmUnsubscribeQuery : IRequest<GetConfirmUnsubscribeResult>
{
    public string? SearchTerm { get; set; }
}